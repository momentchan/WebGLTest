Shader "Unlit/Output"
{
    Properties
    {
        _Mask ("Mask", 2D) = "white" {}
        _Color1("Color1", Color) = (0,0,0,0)
        _Color2("Color2", Color) = (0,0,0,0)
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

            sampler2D _Mask;
            float4 _Color1;
            float4 _Color2;

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_Mask, i.uv);
                
                return lerp(_Color1, _Color2, col.r);
            }
            ENDCG
        }
    }
}
