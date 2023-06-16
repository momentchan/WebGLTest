using mj.gist;
using UnityEngine;
using Unity.Mathematics;
using System.Collections;

public class DynamicStroke : Stroke
{
    private int Segments => generator.Segments;


    private DynamicStrokeGenerator generator;

    private float lifetime;
    private float speed;
    private float length;
    private float displace = 0;
    private int currentSegment = -1;
    private float3 prePos;
    private float3 preVel;
    private float3 currentPos;

    private float3 start, end;

    private float seed;
    private float ratio => 1f * currentSegment / Segments;

    private float3 dir => math.normalizesafe(end - start);

    private void SetPosition(int segment)
    {
        var pos = currentPos;
        var vel = math.normalizesafe(currentPos - prePos);

        for (var i = currentSegment; i < segment; i++)
        {
            if (currentSegment != -1)
            {

            }
            var r = 1f * (i-currentSegment) / (segment - currentSegment);
            var p = math.lerp(prePos, currentPos, r);
            var v = math.lerp(preVel, vel, r);

            rt.SetPixel(i, 0, new Color(p.x, p.y, p.z, 0));
            rt.SetPixel(i, 1, new Color(v.x, v.y, v.z, 0));
        }
        
        vel = Vector3.Lerp(preVel, vel, 0.5f);
        preVel = vel;

        for (var i = segment; i < Segments; i++)
        {
            rt.SetPixel(i, 0, new Color(pos.x, pos.y, pos.z, 0));
            rt.SetPixel(i, 1, new Color(vel.x, vel.y, vel.z, 0));
        }
        rt.Apply();
    }

    public void Setup(DynamicStrokeGenerator generator)
    {
        this.generator = generator;
        seed = UnityEngine.Random.value;

        (start, end) = generator.GetStartEndPoints();

        length = Vector3.Distance(start, end);
        lifetime = generator.GetLifeTime(seed);
        speed = generator.GetSpeed(seed);

        rt = new Texture2D(Segments, 2, TextureFormat.RGBAFloat, false);
        rt.wrapMode = TextureWrapMode.Clamp;
        rt.filterMode = FilterMode.Point;

        currentPos = start;
        prePos = start;
        SetPosition(0);

        block = new Block(GetComponent<MeshRenderer>());
        block.SetTexture("_PositionTex", rt);
        block.Apply();

        StartCoroutine(Draw());
    }

    private IEnumerator Draw()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0, 1f));

        var t = 0f;

        while (t < lifetime)
        {
            t += Time.deltaTime;

            block.SetFloat("_Width", generator.GetWidth(seed));
            block.SetFloat("_FadeIn", Mathf.Clamp01((ratio - generator.FadeIn) / generator.FadeIn));
            block.SetFloat("_Ratio", Mathf.Clamp01((float)currentSegment / Segments));
            block.SetFloat("_LifeDacay", generator.GetLifeDecay(t / lifetime));
            block.SetFloat("_NoiseFrequency", generator.GetNoiseFrequency(seed) * length);
            block.SetFloat("_Strength", generator.GetStrength(seed));
            block.Apply();

            var d = math.normalize(dir + noise.srdnoise(currentPos.xy * generator.positionNoiseFrequency) * generator.positionNoiseScale);
            currentPos += d * Time.deltaTime * speed;

            if (currentSegment < Segments)
            {
                displace += Vector3.Distance(currentPos, prePos);

                var segment = Mathf.FloorToInt((displace / length) * Segments);

                if (currentSegment != segment)
                    SetPosition(segment);
                currentSegment = segment;
                prePos = currentPos;
            }

            yield return null;
        }

        Destroy(rt);
        rt = null;
        generator.Remove(this);
    }
}
