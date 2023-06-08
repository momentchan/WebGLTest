using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StripeController : MonoBehaviour
{
    [SerializeField] private Stripe prefab;

    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float frequency = 0.1f;
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private int count = 20;
    [SerializeField] private float border = 0.1f;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] public Vector3 scale;

    public Vector2 scaleRandom = new Vector2(0.8f, 1.2f);
    [SerializeField] private float up = 0.8f;

    [SerializeField] public float randomOffset = 0.1f;

    private Vector3 left => Camera.main.ViewportToWorldPoint(new Vector3(border, up, 1));
    private Vector3 right => Camera.main.ViewportToWorldPoint(new Vector3(1 - border, up, 1));

    private List<Stripe> stripes = new List<Stripe>();

    public Vector3 GetPosition(int id)
    {
        var ratio = id * 1f / (count - 1);
        var multiplier = curve.Evaluate(ratio);
        var p = Vector3.Lerp(left, right, ratio) ;
        return p + Vector3.right * Mathf.Sin(2 * Mathf.PI * frequency * (1f * id / count) + Time.time * speed) * amplitude * multiplier;
    }

    void Start()
    {

        for (var i = 0; i < count; i++)
        {
            var s = Instantiate(prefab, transform);
            s.Setup(this, i);
            stripes.Add(s);
        }
    }

    void Update()
    {

       
    }
}
