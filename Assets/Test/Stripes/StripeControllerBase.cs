using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public abstract class StripeControllerBase : MonoBehaviour
{
    [SerializeField] protected StripeBase prefab;
    [SerializeField] protected List<StripeData> data;
    [SerializeField] protected int count = 20;
    [SerializeField] protected float depth = 1f;

    public Vector2 scaleRandom = new Vector2(0.8f, 1.2f);

    protected Vector3 left(int type) => Camera.main.ViewportToWorldPoint(new Vector3(data[type].border, data[type].up, depth));
    protected Vector3 right(int type) => Camera.main.ViewportToWorldPoint(new Vector3(1 - data[type].border, data[type].up, depth));
    public float RandomOffset(int type) => data[type].randomOffset;
    protected List<StripeBase> stripes = new List<StripeBase>();


    public abstract Vector3 GetPosition(StripeBase s);

    private float[] CDF;

    private void ComputeCDF()
    {
        var sum = data.Select(d => d.prob).Sum();
        var nmls = data.Select(d => d.prob / sum).ToList();
        CDF = new float[data.Count];
        for (var i = 0; i < data.Count; i++)
        {
            CDF[i] = i == 0 ? nmls[i] : CDF[i - 1] + nmls[i];
        }
    }

    private int GetRandomData()
    {
        var value = UnityEngine.Random.value;
        for (var i = 0; i < data.Count; i++)
        {
            if (value < CDF[i])
                return i;
        }
        return 0;
    }

    protected virtual void Start()
    {
        ComputeCDF();

        for (var i = 0; i < count; i++)
        {
            var type = GetRandomData();
            var s = Instantiate(prefab, transform);
            s.Setup(this, i, type, data[type].mat);
            stripes.Add(s);
        }
    }

    public virtual Vector3 GetScale(float seed, int type)
    {
        return scaleRandom.Lerp(seed) * data[type].scale;
    }

    [System.Serializable]
    public class StripeData
    {
        [SerializeField] public Material mat;
        [SerializeField] public float randomOffset = 0.1f;
        [SerializeField] public float border = 0.1f;
        [SerializeField] public float up = 0.8f;
        [SerializeField] public Vector3 scale;
        [SerializeField] public float prob;
    }
}
