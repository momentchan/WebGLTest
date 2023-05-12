Shader "Unlit/VoronoiBasic"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("_NoiseTex", 2D) = "white" {}
        _VoronoiTex("_VoronoiTex", 2D) = "white" {}
        _PointTex ("_PointTex", 2D) = "white" {}
        
        _ScratchTex("_ScratchTex", 2D) = "white" {}
        _ScratchLineTex ("_ScratchLineTex", 2D) = "white" {}
        _EdgeThreshold("_EdgeThreshold", float) = 0

        _DistortionStrength("_DistortionStrength", Range(0, 1)) = 0
         _Scale ("Scale", Range(0.1, 10)) = 1
        _Octaves ("Octaves", Range(1, 8)) = 4
        _Lacunarity ("Lacunarity", Range(1, 4)) = 2
        _Persistence ("Persistence", Range(0, 1)) = 0.5

        _ScratchColor("_ScratchColor", COLOR) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100


        CGINCLUDE
        #define roundness 0.01
        #include "UnityCG.cginc"
        #include "Assets/Packages/unity-gist/Cginc/Fbm.cginc"
        #include "Noise.cginc"

        sampler2D _MainTex;
        sampler2D _PointTex;
        sampler2D _NoiseTex;
        sampler2D _ScratchTex;
        sampler2D _ScratchLineTex;
        int _PointCount;
        float _EdgeThreshold;

        float _Scale;
        int _Octaves;
        float _Lacunarity;
        float _Persistence;
        float _DistortionStrength;
        float4 _ScratchColor;
        ENDCG

        // voronoi
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            float smin( float a, float b, float k )
            {
                float h = clamp( 0.5+0.5*(b-a)/k, 0.0, 1.0 );
                return lerp( b, a, h ) - k*h*(1.0-h);
            }
            
            // (distance, edge, id, id2)
            float4 voronoi(float2 x)
            {
                float2 cp = 0;
                float cd = 10000;
                float id = -1;

                for (int i = 0; i < _PointCount; i++)
                {
                    float2 p = tex2D(_PointTex, float2(i * 1.0/_PointCount, 0));
                    float distanceToCurrent = distance(x, p);
                    if (distanceToCurrent <= cd)
                    {
                        cd = distanceToCurrent;
                        cp = p;
                        id = i;
                    }
                }

                float e = cd;

                float cd2 = 10000;
                float id2 = id;

                for (int i = 0; i < _PointCount; i++)
                {
                    if (i != id) {
                        float2 p = tex2D(_PointTex, float2(i * 1.0 / _PointCount, 0));
                        float distanceToCurrent = distance(x, p);


                        float2 mid = (p + cp) * 0.5;

                        float num = dot(normalize(p - cp), x - mid);

                        e = smin(e, abs(num), roundness);

                        if (distanceToCurrent < cd2)
                        {
                            cd2 = distanceToCurrent;
                            id2 = i;
                        }
                    }
                }
                return float4(cd, 1 - smoothstep(0, 0.05, e), id, id2);
            }

            
            fixed4 frag(v2f_img i) : SV_Target
            {
                float4 v = voronoi(i.uv);
                return v;
            }
            ENDCG
        }

        // EdgeDetection
        Pass
        {
            CGPROGRAM

            #pragma vertex vert_img
            #pragma fragment frag


            fixed4 frag(v2f_img i) : SV_Target
            {
	            float2 n = fbm4(i.uv*1.5, _Time.x);

                float4 d = tex2D(_MainTex, i.uv + n * _DistortionStrength);
                
                float4 color1 = tex2D(_PointTex, float2(d.b * 1.0 / _PointCount, 0.5));
                float4 color2 = tex2D(_PointTex, float2(d.a * 1.0 / _PointCount, 0.5));


                float4 col = d.r * lerp(color1, (color1+color2)*0.5, d.g);


                float scratch = tex2D(_ScratchTex, i.uv + n * 0)*0.3;
                float lineT = tex2D(_ScratchLineTex, i.uv);

                //col = /*pow(d.r, 1) **/ lerp(0, 1, d.g);
                return col  + lerp(0, _ScratchColor, scratch) + lineT * d.g * 1.5 * col;
            }

            ENDCG
        }
    }
}