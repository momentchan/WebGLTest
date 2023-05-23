Shader "Unlit/StrokeMovement"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PositionTex("PositionTex", 2D) = "white" {}
        _Width("Width", float) = 0.1
        _P1("P1", Vector) = (0,0,0,0)
        _P2("P2", Vector) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Assets/Packages/unity-gist/Cginc/Noise.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _PositionTex;

            float4 _MainTex_ST;
            float4 _P1, _P2;
            float _Width;

            v2f vert (appdata v)
            {
                v2f o;

                float4 dir = normalize(tex2Dlod(_PositionTex, float4(v.uv.x, 0.75, 0.0, 1.0)));
                float4 orth = normalize(float4(-dir.y, dir.x, 0, 0));

                float4 wpos = tex2Dlod(_PositionTex, float4(v.uv.x, 0.25, 0.0, 1.0));
                //float4 wpos = lerp(_P1, _P2, v.uv.x);

                wpos += orth * lerp(-1, 1, v.uv.y) * _Width;
                //wpos += orth * snoise(float2(v.uv.x*12.3, _Time.y*0.2))*0.01;

                float4 lpos = mul(unity_WorldToObject, wpos);
                //lpos = v.vertex;//mul(unity_WorldToObject, wpos);
                o.vertex = UnityObjectToClipPos(lpos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv).a;
                col.a *= smoothstep(0, 0.1, i.uv.x) * smoothstep(1, 0.9, i.uv.x);
                col.a *= smoothstep(0, 0.1, i.uv.y) * smoothstep(1, 0.9, i.uv.y);
                //return fixed4(i.uv,0,1);  
                return col;
            }
            ENDCG
        }
    }
}
