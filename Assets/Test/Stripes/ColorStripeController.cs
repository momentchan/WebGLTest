using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorStripeController : StripeControllerBase
{

    [SerializeField] private float amplitude = 0.1f;
    [SerializeField] private float frequency = 0.1f;
    [SerializeField] private float speed = 1.0f;

    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AnimationCurve scaleCurve;

    public override Vector3 GetPosition(int id, int type)
    {
        var ratio = id * 1f / (count - 1);
        var multiplier = curve.Evaluate(ratio);
        var p = Vector3.Lerp(left(type), right(type), ratio);
        return p + Vector3.right * Mathf.Sin(2 * Mathf.PI * frequency * (1f * id / count) + Time.time * speed) * amplitude * multiplier;
    }
    public override Vector3 GetScale(float seed, int type)
    {
        return scaleCurve.Evaluate(seed) * data[type].scale;
    }
}
