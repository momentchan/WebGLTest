using mj.gist;
using UnityEngine;

public class InteractiveStroke : Stroke
{
    private int Segments => generator.Segments;

    public float displace = 0;

    [SerializeField] private int currentSegment = -1;
    private InteractiveStrokeGenerator generator;

    private Drawer Drawer => generator.Drawer;
    public bool Finished { get; internal set; } = false;

    private float length;
    private Vector3 prePos;
    private float seed;
    private float ratio => 1f * currentSegment / Segments;

    private void SetPosition(int segment)
    {
        var pos = Drawer.transform.position;
        var vel = (Drawer.transform.position - prePos).normalized;
        for (var i = segment; i < Segments; i++)
        {
            rt.SetPixel(i, 0, new Color(pos.x, pos.y, pos.z, 0));
            rt.SetPixel(i, 1, new Color(vel.x, vel.y, vel.z, 0));
        }
        rt.Apply();
    }

    public void Setup(InteractiveStrokeGenerator generator)
    {
        this.generator = generator;
        seed = Random.value;

        length = generator.GetLength(seed);

        rt = new Texture2D(Segments, 2, TextureFormat.RGBAFloat, false);
        rt.wrapMode = TextureWrapMode.Clamp;

        prePos = Drawer.transform.position;

        block = new Block(GetComponent<MeshRenderer>());
        block.SetTexture("_PositionTex", rt);
        block.Apply();
    }

    void Update()
    {
        block.SetFloat("_Width", generator.GetWidth(seed));
        block.SetFloat("_FadeIn", Mathf.Clamp01((ratio - generator.fadeIn) / generator.fadeIn));
        block.SetFloat("_FadeOut", Mathf.Clamp01((ratio - generator.fadeOut) / (1-generator.fadeOut)));
        block.SetFloat("_Strength", generator.Drawer.Speed);
        block.Apply();

        if (currentSegment < Segments)
        {
            displace += Vector3.Distance(Drawer.transform.position, prePos);

            var segment = Mathf.FloorToInt((displace / length) * Segments);

            if (currentSegment != segment)
                SetPosition(segment);

            currentSegment = segment;

            prePos = Drawer.transform.position;
        }
        else
        {
            Finished = true;
        }
    }
}
