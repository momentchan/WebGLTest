Shader "Unlit/StrokeFluid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Noise.cginc"

            sampler2D _MainTex;
            float4 _Mouse;
            float4 _TexSize;
            float4 _BrushData;
            float _BrushSize;
            
            fixed4 frag (v2f_img i) : SV_Target
            {
                float2 fragCoord  = i.uv * _TexSize.xy;
                float2 uv = i.uv;

                float4 brushData = _BrushData;
                float brushStrength = brushData.w;
    
                float4 col = tex2D(_MainTex, uv); 

                float2 w = _TexSize.zw;
                float4 tr = tex2D(_MainTex, uv + float2(w.x , 0));
                float4 tl = tex2D(_MainTex, uv - float2(w.x , 0));
                float4 tu = tex2D(_MainTex, uv + float2(0 , w.y));
                float4 td = tex2D(_MainTex, uv - float2(0 , w.y));

                float3 dx = (tr.xyz - tl.xyz)*0.5;
                float3 dy = (tu.xyz - td.xyz)*0.5;
                float2 densDif = float2(dx.z ,dy.z);

                const float K = 0.2;
                const float v = 0.55;
                float dt = 0.2;
                col.z -= _Time*dot(float3(densDif, dx.x + dy.y) ,col.xyz);
                float2 laplacian = tu.xy + td.xy + tr.xy + tl.xy - 4.0*col.xy;
                float2 viscForce = float2(v, v)*laplacian;
                col.xyw = tex2D(_MainTex, uv - _Time*col.xy*w).xyw;



                float2 dirVec = normalize(_Mouse.xy - brushData.xy);
                dirVec = float2(dirVec.y, -dirVec.x);
                float vd = dot(fragCoord, dirVec);
    
                float brushStregthRamp = clamp(sin(brushStrength * 3.1415926), 0.0, 1.0);
    
                for(int i = 0; i < 20; i++) {
                    float2 mPos = lerp(_Mouse.xy, brushData.xy, float(i) / 20.0);
                    float d = 1.0 - smoothstep(0.0, 1.0, distance(fragCoord, mPos) / _BrushSize / brushStregthRamp);
                    d *= smoothstep(0.2, 1.0, (noise(fragCoord - mPos) + 0.75) / 5.0) * 10.0;               
                    float bd = 1.0 - smoothstep(0.0, 1.0, distance(fragCoord, mPos) / _BrushSize / brushStregthRamp / 1.5  );
                    
                    bd *= smoothstep(0.1, 1.0, (noise(float2(brushStrength * 20.0, 0.0) + (fragCoord - mPos) / 2.0) + 0.0) / 5.0) * 10.0;               
                    col += d + bd;
                }
    
                return col;
            }
            ENDCG
        }
    }
}