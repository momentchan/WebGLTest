#ifndef __NOISE_INCLUDE__
#define __NOISE_INCLUDE__

float rand(float2 n) { 
    return frac(sin(dot(n, float2(12.9898, 4.1414))) * 43758.5453);
}

float noise(float2 p) {
    float2 ip = floor(p);
    float2 u = frac(p);
    u = u*u*(3.0-2.0*u);
    
    float res = lerp(
        lerp(rand(ip), rand(ip+float2(1.0,0.0)), u.x),
        lerp(rand(ip+float2(0.0,1.0)), rand(ip+float2(1.0,1.0)), u.x), u.y);
    return res*res;
}
#endif