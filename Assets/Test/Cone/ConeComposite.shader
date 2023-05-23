Shader "Unlit/ConeComposite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BackgroundTex("BackgroundTex", 2D) = "white" {}
        _ScratchTex("ScratchTex", 2D) = "white" {}
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
            #include "Assets/Packages/unity-gist/Cginc/PhotoShopMath.cginc"

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _BackgroundTex;
            sampler2D _BlurTex;
            sampler2D _ScratchTex;


            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 bg = tex2D(_BackgroundTex, i.uv);
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 blured = tex2D(_BlurTex, i.uv);
                fixed4 scratch = tex2D(_ScratchTex, i.uv );


                col += blured;

                col.rgb = BlendColorDodge(col.rgb, bg.rgb);
                col += bg;

                col.rgb = BlendSoftLight(col.rgb, scratch);
                return col;// + blured;
            }
            ENDCG
        }
    }
}
