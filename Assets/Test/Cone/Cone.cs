using System;
using System.Collections;
using mj.gist;
using UnityEngine;

public class Cone : MonoBehaviour
{
    protected Block block;

    protected ConeGenerator generator;
    protected float seed;

    protected float t = 0;
    protected float T;

    protected float ratio => t / T;
    public void Setup(ConeGenerator generator)
    {
        block = new Block(GetComponent<MeshRenderer>());
        this.generator = generator;
        Reset();
        StartCoroutine(Show());
    }

    protected virtual void Reset()
    {
        seed = UnityEngine.Random.value;
        transform.position = generator.GetRandomPosition();
        transform.rotation = Quaternion.Euler(0, 0, generator.rotation.Lerp(seed));
        
        t = 0;
        T = generator.duration.Lerp(seed);
    }

    protected virtual void Update()
    {
        transform.localScale = new Vector3(generator.scaleX.Lerp(seed),
                                            generator.scaleY.Lerp(seed),
                                            1);
        block.SetVector("_Shift", new Vector2(generator.shift.Lerp(seed), 0));
        block.SetVector("_Radius", new Vector2(generator.radiusMin.Lerp(seed), generator.radiusMax.Lerp(seed)));
        block.SetFloat("_DecayPower", generator.decayPower.Lerp(seed));
        block.SetFloat("_Frequency", generator.frequency.Lerp(seed));
        block.SetColor("_Color", Color.Lerp(generator.color1, generator.color2, seed));
        block.SetFloat("_Seed", seed);
        block.SetFloat("_Ratio", ratio);
        block.Apply();

    }

    IEnumerator Show()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 3f));

        while (true)
        {
            if (t < T)
            {
                t += Time.deltaTime;
            }
            else
            {
                Reset();
            }
            yield return null;
        }
    }
}
