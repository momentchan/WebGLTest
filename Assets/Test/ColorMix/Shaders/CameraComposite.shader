Shader"Unlit/CameraComposite"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OverlayTex("OverlayTex", 2D) = "white" {}
        _OverlayStrength("OverlayStrength", float) = 0.1
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
            sampler2D _OverlayTex;
            float4 _MainTex_ST;
            float _OverlayStrength;
            

            fixed4 frag (v2f_img i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed overlay = tex2D(_OverlayTex, i.uv).r*2;
    
                col.rgb = BlendOverLay(col.rgb, overlay.rrr);
                return col;
            }
            ENDCG
        }
    }
}
