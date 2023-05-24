using System.Collections;
using mj.gist;
using UnityEngine;

public class Stroke : MonoBehaviour
{
    private Texture2D rt;
    private StrokeGenerator generator;

    private Block block;
    private float seed;

    private float life => startT + keepT + endT;
    private float startT;
    private float keepT;
    private float endT;

    public void Setup(StrokeGenerator generator)
    {
        this.generator = generator;
        seed = Random.value;
        rt = generator.GetPositionTexture();

        startT = generator.GetStartT(seed);
        keepT = generator.GetKeepT(seed);
        endT = generator.GetEndT(seed);

        block = new Block(GetComponent<MeshRenderer>());
        block.SetTexture("_PositionTex", rt);
        block.SetFloat("_Seed", seed);
        block.Apply();

        StartCoroutine(Work());
    }

    private void Update()
    {
        block.SetFloat("_Width", generator.GetWidth(seed));
        block.Apply();
    }

    IEnumerator Work()
    {
        yield return null;
        var t = 0f;
        while (t < life)
        {
            t += Time.deltaTime;
            if (t < startT)
            {
                block.SetFloat("_FadeIn", t / startT);
                block.Apply();
            }

            if (t > startT + keepT)
            {
                block.SetFloat("_FadeOut", (t - (startT + keepT)) / endT);
                block.Apply();
            }
            yield return null;
        }
        generator.Remove(this);
    }
}
