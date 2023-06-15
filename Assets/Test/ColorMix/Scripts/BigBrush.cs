using System.Collections;
using mj.gist;
using UnityEngine;

public class BigBrush : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    private Block block;

    private BigBrushGenerator generator;
    private float life;

    public void Setup(BigBrushGenerator generator)
    {
        this.generator = generator;
        life = generator.Lifetime.GetRandom();
    }

    void Start()
    {
        block = new Block(GetComponentsInChildren<MeshRenderer>());
        StartCoroutine(Draw());
    }

    IEnumerator Draw()
    {
        yield return null;
        var t = 0f;
        var r = 0f;

        while (t < life)
        {
            t += Time.deltaTime;
            r += Time.deltaTime * speed;
            block.SetFloat("_DrawRatio", r);
            block.SetFloat("_Fade", generator.FadeCurve.Evaluate(t / life));
            block.Apply();

            yield return null;
        }
        generator.Remove(this);
    }
}
