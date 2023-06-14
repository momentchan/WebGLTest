#include "Assets/Packages/unity-gist/Cginc/Shape.cginc"
#include "Assets/Packages/unity-gist/Cginc/Random.cginc"
#include "Assets/Packages/unity-gist/Cginc/SimplexNoise2D.cginc"
#include "Assets/Packages/unity-gist/Cginc/PhotoshopMath.cginc"

void DrawBigBrush_float(float2 UV, float2 Center, float Count, float Width, float Height, float ratio, float strength, out float Out)
{
    Out = 0;
    for (int i = 0; i < Count; i++)
    {
        float n = nrand(float2(i * 0.45, i * 1.89));
        
        float offset = snoise(float2(n * 34.5, UV.y*2)) * 0.005;
        
        float2 size = frac(float2(n * 43.4, n * 123.8));
        
        
        float x = lerp(0, 1, n);
        float y = lerp(0.2, 0.8, frac(n * 133));
        float w = lerp(0.5, 1.5, size.x) * Width;
        float h = lerp(0.3, 1, size.y) * Height;
        
        float delay = -lerp(0, 0.5, frac(n * 32.5));
        float speed = lerp(0.6, 1, frac(n * 69.3));
        
        
        float r = ratio * speed;
        float fade = 1 - smoothstep(delay + r, delay + r + 0.05, UV.y);
        
        float o = drawEllipse(UV + offset, float2(x, y), w, h) * strength * fade;
        Out += o;
        Out = saturate(Out);
        
        //Out = BlendScreen(Out.rrr, o.rrr);
    }
    Out = saturate(Out) * 0.5;
    
}