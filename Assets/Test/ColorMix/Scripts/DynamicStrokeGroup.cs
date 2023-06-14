using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicStrokeGroup : StrokeGenerator
{
    [SerializeField] private DynamicStroke stroke;

    [SerializeField] private Vector2 length = new Vector2(0.2f, 0.5f);
    [SerializeField] private Vector2 width = new Vector2(0.03f, 0.1f);

    [SerializeField] private Vector2 life = new Vector2(3f, 10f);
    [SerializeField] private float lifeDecay = 0.8f;
    [SerializeField] private Vector2 speed = new Vector2(0.02f, 0.1f);

    [SerializeField] private Vector2 noiseFrequency;

    [SerializeField] private Vector2 strokeCount = new Vector2(3, 10);
    [SerializeField] private float offsetRange = 0.05f;

    private Vector3 startViewPos, endViewPos;
    private Vector3 StartWorldPos => Camera.main.ViewportToWorldPoint(startViewPos);
    private Vector3 EndWorldPos => Camera.main.ViewportToWorldPoint(endViewPos);

    public float smoothness = 0.5f;
    public int Segments = 200;

    public float GetLifeDecay(float ratio) => Mathf.Clamp01(1 - (ratio - lifeDecay) / (1 - lifeDecay));

    private List<DynamicStroke> strokes = new List<DynamicStroke>();

    public float GetLife(float seed) => life.Lerp(seed);
    public float GetWidth(float seed) => width.Lerp(seed);
    public float GetSpeed(float seed) => speed.Lerp(seed);
    public float GetNoiseFrequency(float seed) => noiseFrequency.Lerp(seed);

    public float fadeIn = 0.2f;
    public float fadeOut = 0.8f;
    public float GetLength(float seed) => length.Lerp(seed);

    public (Vector3, Vector3) GetStartEndPoints()
    {
        var offset = Random.insideUnitSphere * offsetRange;
        return (StartWorldPos + offset, EndWorldPos + offset);
    }

    void Start()
    {
        SpawnStrokes();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnStrokes();
        }
    }

    private void SpawnStrokes()
    {
        startViewPos = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), 1);
        endViewPos = startViewPos + (Vector3)Random.insideUnitCircle * length.RandomInRange();

        var c = strokeCount.RandomInRange();
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
        Gizmos.DrawWireSphere(StartWorldPos, offsetRange);
        Gizmos.DrawWireSphere(EndWorldPos, offsetRange);
    }
}
