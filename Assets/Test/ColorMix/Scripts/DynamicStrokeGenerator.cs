using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicStrokeGenerator : StrokeGenerator
{

    public int Segments => segments;
    public float FadeIn => fadeIn;
    public float GetLifeDecay(float ratio) => Mathf.Clamp01(1 - (ratio - lifeDecay) / (1 - lifeDecay));
    public float GetLifeTime(float seed) => life.Lerp(seed);
    public float GetWidth(float seed) => width.Lerp(seed);
    public float GetSpeed(float seed) => speed.Lerp(seed);
    public float GetNoiseFrequency(float seed) => noiseFrequency.Lerp(seed);
    public float GetLength(float seed) => length.Lerp(seed);
    public (Vector3, Vector3) GetStartEndPoints()
    {
        var offset = Random.insideUnitSphere * this.offset;
        return (startPos + offset, endPos + offset);
    }

    [SerializeField] private DynamicStroke stroke;

    [Header("Spawn")]
    [SerializeField] private float offset = 0.05f;
    [SerializeField] private Vector2 strokeCount = new Vector2(3, 10);
    [SerializeField] private Vector2 spawnDuration = new Vector2(1f, 6f);

    [Header("Life")]
    [SerializeField] private float lifeDecay = 0.8f;
    [SerializeField] private Vector2 life = new Vector2(3f, 10f);

    [Header("Motion")]
    [SerializeField] public float positionNoiseScale = 1;
    [SerializeField] public float positionNoiseFrequency = 1;
    [SerializeField] private Vector2 speed = new Vector2(0.02f, 0.1f);
    [SerializeField] private Vector2 noiseFrequency;

    [Header("Render")]
    [SerializeField] private int segments = 200;
    [SerializeField] private float fadeIn = 0.2f;
    [SerializeField] private Vector2 length = new Vector2(0.2f, 0.5f);
    [SerializeField] private Vector2 width = new Vector2(0.03f, 0.1f);

    private List<DynamicStroke> strokes = new List<DynamicStroke>();
    private Vector3 startPos, endPos;

    void Start()
    {
        StartCoroutine(Work());
    }

    private IEnumerator Work()
    {
        yield return null;
        while (true)
        {
            SpawnStrokes();
            yield return new WaitForSeconds(spawnDuration.GetRandom());
        }
    }

    private void SpawnStrokes()
    {
        startPos = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1));
        endPos = startPos + Random.insideUnitSphere * length.GetRandom();

        var c = strokeCount.GetRandom();
        for (var i = 0; i < c; i++)
        {
            var s = Instantiate(stroke, transform);
            s.Setup(this);
            strokes.Add(s);
        }
    }

    public void Remove(DynamicStroke s)
    {
        strokes.Remove(s);
        Destroy(s.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPos, offset);
        Gizmos.DrawWireSphere(endPos, offset);
    }
}
