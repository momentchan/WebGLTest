using System.Collections;
using System.Collections.Generic;
using BezierTools;
using UnityEngine;

public class StrokeGenerator : MonoBehaviour
{
    [SerializeField] Bezier bezier;
    [SerializeField] private Stroke stroke;
    [SerializeField] private float speed = 0.1f;

    [SerializeField] private Vector2 spawnT = new Vector2(3f, 5f);
    [SerializeField] private Vector2 startT = new Vector2(2f, 3f);
    [SerializeField] private Vector2 keepT = new Vector2(2f, 3f);
    [SerializeField] private Vector2 endT = new Vector2(2f, 3f);
    [SerializeField] private Vector2 width = new Vector2(0.05f, 0.1f);
    [SerializeField] private Vector2 length = new Vector2(0.1f, 0.15f);

    public float GetStartT(float seed) => RandomUtil.RandomInRange(startT, seed);
    public float GetKeepT(float seed) => RandomUtil.RandomInRange(keepT, seed);
    public float GetEndT(float seed) => RandomUtil.RandomInRange(endT, seed);
    public float GetWidth(float seed) => RandomUtil.RandomInRange(width, seed);

    private readonly int sampleCount = 100;
    private List<Stroke> strokes = new List<Stroke>();
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
        yield return null;
        while (true)
        {
            SpawnStroke();
            yield return new WaitForSeconds(Random.Range(spawnT.x, spawnT.y));
        }
    }

    private void SpawnStroke()
    {
        var s = Instantiate(stroke, transform);
        s.Setup(this);
        strokes.Add(s);
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

    public void Remove(Stroke s)
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