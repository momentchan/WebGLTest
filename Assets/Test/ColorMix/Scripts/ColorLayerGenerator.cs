using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColorLayerGenerator : MonoBehaviour
{
    [SerializeField] private ColorLayer layer;
    private List<ColorLayer> layers = new List<ColorLayer>();
    [SerializeField] private Vector2 spawnRate = new Vector2(5f, 10f);
    [SerializeField] public Vector2 lifetime = new Vector2(5f, 10f);
    [SerializeField] private AnimationCurve alphaCurve;

    public float GetAlpha(float ratio) => alphaCurve.Evaluate(ratio);



    void Start()
    {
        StartCoroutine(Work());
    }

    private IEnumerator Work()
    {
        yield return null;
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnRate.GetRandom());
        }
    }
    private void Spawn()
    {
        var pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 1));

        var l = Instantiate(layer, transform);
        l.transform.position = pos;
        l.Setup(this);
        layers.Add(l);
    }

    internal void Remove(ColorLayer colorLayer)
    {
        layers.Remove(colorLayer);
        Destroy(colorLayer.gameObject);
    }
}
