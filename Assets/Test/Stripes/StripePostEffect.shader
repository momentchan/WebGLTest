Shader "Unlit/StripePostEffect"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _StrokeTex ("_StrokeTex", 2D) = "white" {}
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
            float4 _MainTex_ST;

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed stroke = tex2D(_StrokeTex, i.uv);
                //col.rgb *= stroke;// BlendLinearBurn(col, stroke.rrrr);
                
                return col;
            }
            ENDCG
        }
    }
}
