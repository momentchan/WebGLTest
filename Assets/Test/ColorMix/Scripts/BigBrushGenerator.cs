using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

public class BigBrushGenerator : MonoBehaviour
{
    public Vector2 Lifetime => lifeTime;
    public AnimationCurve FadeCurve => fadeCurve;

    [SerializeField] private BigBrush prefab;
    [SerializeField] private Vector2 spawnFrequency = new Vector2(3, 5);
    [SerializeField] private Vector2 lifeTime = new Vector2(5, 10);
    [SerializeField] private Vector2 size = new Vector2(0.5f, 1.5f);
    [SerializeField] private AnimationCurve fadeCurve;



    private List<BigBrush> brushes = new List<BigBrush>();

    private void Start()
    {
        StartCoroutine(Generate());
    }

    IEnumerator Generate()
    {
        yield return null;
        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnFrequency.GetRandom());
        }
    }

    void Spawn()
    {
        var b = Instantiate(prefab, transform);
        b.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(Random.value, Random.value, 1));
        b.transform.localScale *= size.GetRandom();
        b.transform.eulerAngles = new Vector3(Random.value * 360f, -90, 90);
        b.Setup(this);
        brushes.Add(b);
    }

    internal void Remove(BigBrush bigBrush)
    {
        brushes.Remove(bigBrush);
        Destroy(bigBrush.gameObject);
    }
}
