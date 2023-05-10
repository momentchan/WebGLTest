Shader "Unlit/VoronoiCrack"
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
            #include "Noise.cginc"

            sampler2D _MainTex;
            float4 _Mouse;
            float4 _TexSize;
            float4 _BrushData;
            float _BrushSize;
            
            #define VARIANT 1              // 1: amplifies Voronoi cell jittering
            #if VARIANT
                float ofs = .5;          // jitter Voronoi centers in -ofs ... 1.+ofs
            #else
                float ofs = 0.;
            #endif

            // === pseudo Perlin noise =============================================
            #define rot(a) float2x2(cos(a),-sin(a),sin(a),cos(a))
           
    

            // std int hash, inspired from https://www.shadertoy.com/view/XlXcW4
            float3 hash3(uint3 x)
            {
                #define scramble  x = ( (x>>8U) ^ x.yzx ) * 1103515245U // GLIB-C const
                scramble; scramble; scramble;
                return float3(x) / float(0xffffffffU) + 1e-30; // <- eps to fix a windows/angle bug
            }

            #define hash22(p)  frac( 18.5453 * sin( p * float2(127.1,311.7)) )
            #define disp(p) ( -ofs + (1.+2.*ofs) * hash22(p) )

            float3 VoronoiB(float2 u)
            {
                float2 iu = floor(u);
                float2 C, P;
                float m = 1e9;
                float d;

                #if VARIANT
                for (int k = 0; k < 25; k++)
                {
                    float2 p = iu + float2(k % 5 - 2, k / 5 - 2);
                #else
                for (int k = 0; k < 9; k++)
                {
                    float2 p = iu + float2(k % 3 - 1, k / 3 - 1);
                #endif
                    float o = disp(p);
                    float2 r = p - u + o;
                    d = dot(r, r);
                    if (d < m)
                    {
                        m = d;
                        C = p - iu;
                        P = r;
                    }
                }

                m = 1e9;

                for (int k = 0; k < 25; k++)
                {
                    float2 p = iu + C + float2(k % 5 - 2, k / 5 - 2);
                    float o = disp(p);
                    float2 r = p - u + o;

                    if (dot(P - r, P - r) > 1e-5)
                        m = min(m, 0.5 * dot((P + r), normalize(r - P)));
                }

                return float3(m, P + u);
            }


        float2 hash21(float2 p)
        {
            return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
        }

        float noise2(float2 p)
        {
            float2 i = floor(p);
            float2 f = frac(p);
            f = f * f * (3.0 - 2.0 * f); // smoothstep

            float v = lerp(
                lerp(hash21(i + float2(0, 0)), hash21(i + float2(1, 0)), f.x),
                lerp(hash21(i + float2(0, 1)), hash21(i + float2(1, 1)), f.x),
                f.y
            );
            return 2.0 * v - 1.0;
        }

        float2 noise22(float2 p)
        {
            return float2(noise2(p), noise2(p + 17.7));
        }

        float2 fbm22(float2 p)
        {
            float2 v = float2(0,0);
            float a = 0.5;
            float2x2 R = rot(.37);

            for (int i = 0; i < 6; i++, p *= 2.0, a /= 2.0)
            {
                p = mul(p,R);
                v += a * noise22(p);
            }

            return v;
        }

            fixed4 frag (v2f_img i) : SV_Target
            {
                 float RATIO = 1.,              // stone length/width ratio  
                CRACK_depth = 1.,
                CRACK_zebra_scale = 1.,  // fractal shape of the fault zebra
                CRACK_zebra_amp = .67,
                CRACK_profile = 1.,      // fault vertical shape  1.  .2 
                CRACK_slope = 50.,       //                      10.  1.4
                CRACK_width = .0;


                float4 O = 0;
                float2 U = i.uv * 3.0;
                float2 I = floor(U / 2.0);
                float4 Z = 0;
                Z.a=1;
               
                float3 H0;
                O -= O;

                for (float i = 0.0; i < CRACK_depth; i++)
                {
                    float2 V = U / float2(RATIO, 1.0);                  // voronoi cell shape
                    float2 D = 0;//CRACK_zebra_amp * fbm22(U / CRACK_zebra_scale) * CRACK_zebra_scale;
                    float3 H = VoronoiB(V + D);
                    if (i == 0.0)
                        H0 = H;
                    float d = H.x;                                // distance to cracks

                    d = min(1.0, CRACK_slope * pow(max(0.0, d - CRACK_width), CRACK_profile));

                    O =H.x;// fixed4(1.0 - d, 1.0 - d, 1.0 - d, 1.0) / exp2(i);
                    U = mul(1.5 * rot(0.37), U);
                }
    
                #if MM
                    O.g = hash3(uint3(H0.yz, 1)).x;
                #endif
                return O;
            }
            ENDCG
        }
    }
}