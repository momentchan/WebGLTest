Shader "Unlit/Mountain"
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

            #define ss(a, b, t) smoothstep(a, b, t)
            #define rot(a) float2x2(cos(a), -sin(a), sin(a), cos(a))
            
            #define rad 0.016
            #define it 4.0
            #define v float3(1.0, 0.0, 1.0)

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float2 hash22(float2 x) {
                const float2 k = float2(0.3183099, 0.3678794);
                x = x * k + k.yx;
                return -1.0 + 2.0 * frac(16.0 * k * frac(x.x * x.y * (x.x + x.y)));
            }
            float hsh(float2 p) {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            float perlin(float2 p) {
                float2 i = floor(p);
                float2 f = frac(p);
    
                float a = hsh(i);
                float b = hsh(i + float2(1.0, 0.0));
                float c = hsh(i + float2(0.0, 1.0));
                float d = hsh(i + float2(1.0, 1.0));
    
                float2 u = smoothstep(0.0, 1.0, f);
    
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            
            float octnse(float2 p, int oct, float t) {
                float a = 1.0;
                float n = 0.0;
                
                for (int i = 0; i < oct; i++) {
                    p.x += t;
                    n += perlin(p) * a;
                    p *= 2.0;
                    a *= 0.5;
                }
                
                return n;
            }

            float smoothmin(float d1, float d2, float k) {
                float h = clamp(0.5 + 0.5 * (d2 - d1) / k, 0.0, 1.0);
                return lerp(d2, d1, h) - k * h * (1.0 - h);
            }

            float drawLine(float3 p, float3 a, float3 b, float r) {
                float3 pa = p - a;
                float3 ba = b - a;
                float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
                return length(pa - ba * h) - r;
            }

            float gh(float2 rp) {
                float h = 0.0;
    
                float t = rp.x + rp.y * 1.2;
                h += cos(rp.y) + 0.5 * cos(t * 4.0) + 0.3 * sin(rp.y * 3.0);
                h += cos(rp.x) + 0.4 * sin(rp.x * 2.0) + 0.3 * cos(t * 3.0);
                h *= 0.3;
    
                return h;
            }

            float trees_kinda(float3 p) {
                float scl = 0.33;
                float d = 999.0;
                float ts = 1.0;
    
                for (float i = 0.0; i < it; i++) {
                    p.xz = -abs(p.xz);
                    float a = 2.9;
        
                    a -= cos(i) * 0.15;
                    p.y -= scl * 2.0;
        
                    p.y += scl;
                    p.xy = mul(rot(a), p.xy);
                    p.yz = mul(rot(a), p.yz);
                    p.y -= scl;
        
                    scl *= 0.8;
                }
    
                d = smoothmin(d, length(p - float3(0.0, ts, 0.0)) - 0.23, 0.7);
                return d;
            }

            float map(float3 rp) {
                rp.z += _Time.y;

                float h = gh(rp.xz);
    
                h -= 0.1 * octnse(rp.xz * 3.8, 3, 0.0);
                h -= cos(rp.z * 2.0) * 0.13;

                float d = rp.y + h;
    
                rp.xz = mul(rot(3.14 / 4.0), rp.xz);
                d = smoothmin(trees_kinda(fmod(rp - float3(1.0, -3.4 + h, 0.0), v) - v * 0.5), d, 0.9);
    
                return d;
            }

            float3 normal(float3 pos) {
                float2 e = float2(0.002, -0.002);
                return normalize(
                    e.xyy * map(pos + e.xyy) + 
                    e.yyx * map(pos + e.yyx) + 
                    e.yxy * map(pos + e.yxy) + 
                    e.xxx * map(pos + e.xxx)
                );
            }
            float4 _Mouse;
            fixed4 frag (v2f_img i) : SV_Target
            {
                float2 uv = (i.uv-0.5)*_ScreenParams.xy / _ScreenParams.x;
                float2 m = 0;

                float3 rd = normalize(float3(uv, 2.4));
                float3 ro = float3(0.0, 2.15 + 0.1 * cos(_Time.y * 0.4), 0.0);

                rd.yz = mul(rot(0.1), rd.yz);
                rd.xz = mul(rot(0.1), rd.xz);

                float d = 0.0;
                float t = 0.0;
                float ns = 0.0;

                for (int i = 0; i < 61; i++) {
                    d = map(ro + rd * t);
                    if (d < 0.0025 || t > 30.0)
                        break;
                    t += d * 0.7;
                    ns++;
                }
                float3 p = ro + rd * t;
                float3 n = normal(p);

                p.z += _Time.y;

                float3 ld = normalize(float3(-0.8, 0.5, -2.0));
                float dif = max(dot(n, ld), 0.1);
                float ao = ss(7.0, 2.0, ns * 0.3);

                float rnd = perlin(p.xz * 2.5);
                float3 grass = lerp(float3(0,0,0), 0.23 * float3(0.1, 0.2, 0.1), rnd);

                float cloud = (1.0 - 0.17 * octnse(rd.xy * 10.0, 4, -_Time.y * 0.12));
                cloud = lerp(cloud, 1.0, ss(19.0, 11.0, t));
                float3 sky = 1; //cloud * float3(0.6, 0.7, 0.95);

                float3 col = grass * dif * ao;

                float fd = 22.0;
                float fog = ss(fd, fd - 16.4, t);

                col = lerp(sky, col, fog);
                col = pow(col * 1.1, 1.4);

                return fixed4(sqrt(clamp(col, 0.0, 1.0)), 1.0);
            }

            ENDCG
        }
    }
}
