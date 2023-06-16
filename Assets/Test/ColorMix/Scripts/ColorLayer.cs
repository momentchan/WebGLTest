using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

[ExecuteInEditMode]
public class ColorLayer : MonoBehaviour
{
    private Block block;
    private float seed;
    private ColorLayerGenerator generator;
    internal void Setup(ColorLayerGenerator generator)
    {
        this.generator = generator;
        seed = Random.value;

        block = new Block(GetComponent<Renderer>());
        block.SetFloat("_Alpha", 0);
        block.SetFloat("_Seed", seed);
        block.Apply();
        StartCoroutine(Show(generator.lifetime.GetRandom()));
    }

    private IEnumerator Show(float lifetime)
    {
        yield return null;
        var t = 0f;

        while (t < lifetime)
        {
            t += Time.deltaTime;
            block.SetFloat("_Alpha", generator.GetAlpha(t / lifetime));
            block.Apply();

            yield return null;
        }

        generator.Remove(this);
    }
}
