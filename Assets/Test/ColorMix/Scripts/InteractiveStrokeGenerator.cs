using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveStrokeGenerator : StrokeGenerator
{
    [SerializeField] private InteractiveStroke stroke;
    [SerializeField] private Drawer drawer;

    [SerializeField] private Vector2 length = new Vector2(0.2f, 0.5f);
    [SerializeField] private Vector2 width = new Vector2(0.03f, 0.1f);

    public float smoothness = 0.5f;
    public int Segments = 200;

    private List<InteractiveStroke> strokes = new List<InteractiveStroke>();
    public float GetWidth(float seed) => width.Lerp(seed);
    public float fadeIn = 0.2f;
    public float fadeOut = 0.8f;
    public Drawer Drawer => drawer;
    public float GetLength(float seed) => length.Lerp(seed);

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return null;
        while (true)
        {
            var s = SpawnStroke();
            yield return new WaitUntil(() => s.Finished);
            Remove(s);
        }
    }

    private InteractiveStroke SpawnStroke()
    {
        var s = Instantiate(stroke, transform);
        s.Setup(this);
        strokes.Add(s);
        return s;
    }

    public void Remove(InteractiveStroke s)
    {
        strokes.Remove(s);
        Destroy(s.gameObject);
    }
}
