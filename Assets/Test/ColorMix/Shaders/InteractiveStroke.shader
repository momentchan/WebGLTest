Shader"Unlit/InteractiveStroke"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PositionTex("PositionTex", 2D) = "white" {}
        _ColorTex("ColorTex", 2D) = "white" {}
        _Noise("Noise", float) = 0
        _Offset("Offset", float) = 0
        _HsvShift("HsvShift", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha One
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_dynamic
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Assets/Packages/unity-gist/Cginc/Noise.cginc"
            #include "Assets/Packages/unity-gist/Cginc/Rotation.cginc"
            #include "Assets/Packages/unity-gist/Cginc/PhotoshopMath.cginc"
                
            #include "StrokeCommon.cginc"

            float _LifeDacay;
            float _Ratio;
            float4 _HsvShift;
            sampler2D _ColorTex;
            fixed4 frag (v2f i) : SV_Target
            {
                float w = 1;
                fixed4 col = tex2D(_MainTex, i.uv).a;
                fixed4 screenColor =  tex2D(_ColorTex, i.screenPos);
                screenColor.rgb = HSVShift(screenColor.rgb, _HsvShift);
    
                float t1 = (_FadeIn * 2 * w) -w;
    

                col.a *= smoothstep(t1 + 1, t1, i.uv.x);
                col.rgb *= screenColor.rgb * _HsvShift.w; 
                
                return col * _Strength * _LifeDacay;
            }

            ENDCG
        }
    }
}
