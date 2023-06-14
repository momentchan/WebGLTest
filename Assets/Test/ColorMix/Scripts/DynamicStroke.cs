using mj.gist;
using Unity.VisualScripting;
using UnityEngine;
using Unity.Mathematics;

public class DynamicStroke : Stroke
{
    private int Segments => generator.Segments;

    public float displace = 0;

    [SerializeField] private int currentSegment = -1;
    private DynamicStrokeGroup generator;

    private float length;
    private float3 prePos;
    private Vector3 preNormal;

    private float seed;
    private float ratio => 1f * currentSegment / Segments;

    private float life;
    private float t;
    private float speed;

    private float3 currentPos;

    private float3 start, end;
    private float3 dir => math.normalizesafe(end - start);


    private void SetPosition(int segment)
    {

        var pos = currentPos;
        var vel = math.normalizesafe(currentPos - prePos);
        vel = Vector3.Lerp(preNormal, vel, 0.5f);
        preNormal = vel;

        for (var i = segment; i < Segments; i++)
        {
            rt.SetPixel(i, 0, new Color(pos.x, pos.y, pos.z, 0));
            rt.SetPixel(i, 1, new Color(vel.x, vel.y, vel.z, 0));
        }
        rt.Apply();
    }

    public void Setup(DynamicStrokeGroup generator)
    {
        this.generator = generator;
        seed = UnityEngine.Random.value;

        (start, end) = generator.GetStartEndPoints();

        length = Vector3.Distance(start, end);
        life = generator.GetLife(seed);
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
    }

    void Update()
    {

        t += Time.deltaTime;

        block.SetFloat("_Width", generator.GetWidth(seed));
        block.SetFloat("_FadeIn", Mathf.Clamp01((ratio - generator.fadeIn) / generator.fadeIn));
        block.SetFloat("_Ratio", Mathf.Clamp01((float)currentSegment / Segments));
        block.SetFloat("_LifeDacay", generator.GetLifeDecay(t / life));
        block.SetFloat("_NoiseFrequency", generator.GetNoiseFrequency(seed) * length);
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

        if (t > life)
        {
            Destroy(rt);
            rt = null;
            generator.Remove(this);
        }
    }
}
