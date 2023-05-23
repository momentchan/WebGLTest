using System;
using System.Collections;
using mj.gist;
using UnityEngine;

public class Cone : MonoBehaviour
{
    private Material mat;
    private Block block;

    private ConeGenerator generator;
    private float seed;

    private float t = 0;
    private float T;

    private float ratio => t / T;
    public void Setup(ConeGenerator generator)
    {
        block = new Block(GetComponent<MeshRenderer>());
        this.generator = generator;
        Reset();
        StartCoroutine(Show());
    }

    private void Reset()
    {
        seed = UnityEngine.Random.value;
        transform.position = generator.GetRandomPosition();
        transform.rotation = Quaternion.Euler(0, 0, RandomUtil.RandomInRange(generator.rotation, seed));
        transform.transform.localScale = new Vector3(RandomUtil.RandomInRange(generator.scaleX, seed),
                                                     RandomUtil.RandomInRange(generator.scaleY, seed),
                                                     1);

        t = 0;
        T = RandomUtil.RandomInRange(generator.duration, seed);
    }

    private void Update()
    {
        block.SetVector("_Shift", new Vector2(RandomUtil.RandomInRange(generator.shift, seed), 0));
        block.SetVector("_Radius", new Vector2(RandomUtil.RandomInRange(generator.radiusMin, seed), RandomUtil.RandomInRange(generator.radiusMax, seed)));
        block.SetFloat("_DecayPower", RandomUtil.RandomInRange(generator.decayPower, seed));
        block.SetFloat("_Frequency", RandomUtil.RandomInRange(generator.frequency, seed));
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
