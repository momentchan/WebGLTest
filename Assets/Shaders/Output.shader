Shader "Unlit/Output"
{
    Properties
    {
        _Texture("_Texture", 2D) = "white" {}
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

            sampler2D _Texture;
            

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_Texture, i.uv);
            
                return col;
            }
            ENDCG
        }
    }
}
