using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBezierStrokeGenerator : StrokeGenerator
{
    [SerializeField] private InteractiveBezierStroke stroke;
    [SerializeField] private Drawer drawer;

    [SerializeField] private Vector2 length = new Vector2(0.2f, 0.5f);
    [SerializeField] private Vector2 width = new Vector2(0.03f, 0.1f);

    [SerializeField] private Vector2 life = new Vector2(3f, 10f);
    [SerializeField] private float lifeDecay = 0.8f;

    public float smoothness = 0.5f;
    public int Segments = 200;

    public float GetLifeDecay(float ratio) => Mathf.Clamp01(1 - (ratio - lifeDecay) / (1 - lifeDecay));

    private List<InteractiveBezierStroke> strokes = new List<InteractiveBezierStroke>();

    public float GetLife(float seed) => life.Lerp(seed);
    public float GetWidth(float seed) => width.Lerp(seed);
    public float fadeIn = 0.2f;
    public float fadeOut = 0.8f;
    public Drawer Drawer => drawer;
    public float GetLength(float seed) => length.Lerp(seed);

    public InteractiveBezierStroke currentStroke;
    public bool IsCurrent(InteractiveBezierStroke stroke) => stroke == currentStroke;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return null;
        while (true)
        {
            currentStroke = SpawnStroke();
            yield return new WaitUntil(() => currentStroke.Finished || currentStroke == null);
        }
    }

    private InteractiveBezierStroke SpawnStroke()
    {
        var s = Instantiate(stroke, transform);
        s.Setup(this);
        strokes.Add(s);
        return s;
    }

    public void Remove(InteractiveBezierStroke s)
    {
        strokes.Remove(s);
        Destroy(s.gameObject);
    }
}
