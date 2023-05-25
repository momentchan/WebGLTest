using System.Collections;
using System.Collections.Generic;
using BezierTools;
using UnityEngine;

public class BezierStrokeGenerator : StrokeGenerator
{
    [SerializeField] Bezier bezier;
    [SerializeField] private BezierStroke stroke;
    [SerializeField] private float speed = 0.1f;

    [SerializeField] private Vector2 spawnT = new Vector2(3f, 5f);
    [SerializeField] private Vector2 startT = new Vector2(2f, 3f);
    [SerializeField] private Vector2 keepT = new Vector2(2f, 3f);
    [SerializeField] private Vector2 endT = new Vector2(2f, 3f);
    [SerializeField] private Vector2 width = new Vector2(0.05f, 0.1f);
    [SerializeField] private Vector2 length = new Vector2(0.1f, 0.15f);
    [SerializeField] private int maxSpawnCount = 3;

    public float GetStartT(float seed) => startT.Lerp(seed);
    public float GetKeepT(float seed) => keepT.Lerp(seed);
    public float GetEndT(float seed) => endT.Lerp(seed);
    public float GetWidth(float seed) => width.Lerp(seed);
    private readonly int sampleCount = 100;
    private List<BezierStroke> strokes = new List<BezierStroke>();
    private float t;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            SpawnStroke();

        t += Time.deltaTime * speed;
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(Random.Range(0, 3f));
        while (true)
        {
            SpawnStroke();
            yield return new WaitForSeconds(Random.Range(spawnT.x, spawnT.y));
        }
    }

    private void SpawnStroke()
    {
        var c = Random.Range(1, maxSpawnCount + 1);
        for (var i = 0; i < c; i++)
        {
            var s = Instantiate(stroke, transform);
            s.Setup(this);
            strokes.Add(s);
        }
    }

    public Texture2D GetPositionTexture()
    {
        var rt = new Texture2D(sampleCount, 2, TextureFormat.RGBAFloat, false);
        rt.wrapMode = TextureWrapMode.Clamp;

        var l = Random.Range(length.x, length.y);
        for (int i = 0; i < sampleCount; i++)
        {
            var nt = (t + i * l / sampleCount) % 1;
            var pos = bezier.data.PositionNormalizeT(nt);
            var vel = bezier.data.VelocityNormalizeT(nt);
            rt.SetPixel(i, 0, new Color(pos.x, pos.y, pos.z, 0));
            rt.SetPixel(i, 1, new Color(vel.x, vel.y, vel.z, 0));
        }
        rt.Apply();

        return rt;
    }

    public void Remove(BezierStroke s)
    {
        strokes.Remove(s);
        Destroy(s.gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        var p = bezier.data.PositionNormalizeT(t % 1);
        var n = bezier.data.VelocityNormalizeT(t % 1);

        Gizmos.DrawWireSphere(p, 0.05f);
        Gizmos.DrawLine(p, p + n * 0.1f);
    }
#endif
}
