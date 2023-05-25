Shader "Unlit/InteractiveStroke"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PositionTex("PositionTex", 2D) = "white" {}
        _T("T", float) = 0
        _Offset("Offset", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Assets/Packages/unity-gist/Cginc/Noise.cginc"
            #include "Assets/Packages/unity-gist/Cginc/Rotation.cginc"

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
            float _Width;
            float _FadeIn;
            float _FadeOut;
            float _Seed;
            float _T;
            float _Offset;

            v2f vert (appdata v)
            {
                v2f o;

                float4 dir = normalize(tex2Dlod(_PositionTex, float4(v.uv.x, 0.75, 0.0, 1.0)));


                float4 orth = normalize(float4(-dir.y, dir.x, 0, 0));

                float4 wpos = tex2Dlod(_PositionTex, float4(v.uv.x, 0.25, 0.0, 1.0));

                wpos += orth * lerp(-1, 1, v.uv.y) * _Width;


                float4 lpos = mul(unity_WorldToObject, wpos);
                o.vertex = UnityObjectToClipPos(lpos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float w = 1;
                fixed4 col = tex2D(_MainTex, i.uv).a;
                float t1 = (_FadeIn * 2 * w) -w;
                float t2 = (_FadeOut * 2 * w) -w;

                col.a *= smoothstep(t1 + 1, t1, i.uv.x);
                col.a *= (1-smoothstep(t2 + 1, t2, i.uv.x));
                return col;
            }
            ENDCG
        }
    }
}
