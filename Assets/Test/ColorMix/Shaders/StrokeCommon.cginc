#ifndef STROKE_COMMON
#define STROKE_COMMON

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
    float4 screenPos : TEXCOORD1;
};

sampler2D _MainTex;
sampler2D _PositionTex;

float4 _MainTex_ST;
float _Width;
float _FadeIn;
float _FadeOut;
float _Seed;
float _Noise;
float _NoiseFrequency;
float _Offset;
float _Strength;

v2f vert (appdata v)
{
    v2f o;

    float4 dir = normalize(tex2Dlod(_PositionTex, float4(v.uv.x, 0.75, 0.0, 1.0)));

    float4 orth = normalize(float4(-dir.y, dir.x, 0, 0));

    float4 wpos = tex2Dlod(_PositionTex, float4(v.uv.x, 0.25, 0.0, 1.0));

    // random offset
    wpos += float4(snoise(float2(_Seed, 0.5)), snoise(float2(0.5, _Seed)), 0, 0)*_Offset;

    wpos += orth * lerp(-1, 1, v.uv.y) * _Width;

    float noise = snoise(float2(v.uv.x*5, _Time.x)) * _Noise;
    noise *= smoothstep(0, 1, v.uv.x) * smoothstep(1, 0, v.uv.x);

    wpos += orth * noise;

    float4 lpos = mul(unity_WorldToObject, wpos);
    o.vertex = UnityObjectToClipPos(lpos);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.screenPos = ComputeScreenPos(o.vertex);
    return o;
}


v2f vert_dynamic(appdata v)
{
    v2f o;

    float4 dir = normalize(tex2Dlod(_PositionTex, float4(v.uv.x, 0.75, 0.0, 1.0)));

    float4 orth = normalize(float4(-dir.y, dir.x, 0, 0));

    float4 wpos = tex2Dlod(_PositionTex, float4(v.uv.x, 0.25, 0.0, 1.0));

    // random offset
    wpos += float4(snoise(float2(_Seed, 0.5)), snoise(float2(0.5, _Seed)), 0, 0) * _Offset;

    wpos += orth * lerp(-1, 1, v.uv.y) * _Width;

    float noise = snoise(float2(v.uv.x * _NoiseFrequency, _Time.x)) * _Noise;
    noise *= smoothstep(0, 1, v.uv.x) * smoothstep(1, 0, v.uv.x);

    wpos += orth * noise;

    float4 lpos = mul(unity_WorldToObject, wpos);
    o.vertex = UnityObjectToClipPos(lpos);
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
    o.screenPos = ComputeScreenPos(o.vertex);
    return o;
}

#endif 