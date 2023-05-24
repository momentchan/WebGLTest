Shader "Unlit/ColorMixOutput"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
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
            #include "Assets/Packages/unity-gist/Cginc/PhotoshopMath.cginc"

            sampler2D _MainTex;
            sampler2D _StrokeTex;
            

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 stroke = tex2D(_StrokeTex, i.uv).r*1;
                col.rgb = BlendSoftLight(col, stroke);

                return col;
            }
            ENDCG
        }
    }
}
