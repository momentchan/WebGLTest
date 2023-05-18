using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

[ExecuteInEditMode]
public class Brush : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float width1;
    [SerializeField] private float strokeCount1;
    [SerializeField, Range(0, 1)] private float width2;
    [SerializeField] private float strokeCount2;
    [SerializeField, Range(0, 1)] private float width3;
    [SerializeField] private float strokeCount3;
    [SerializeField] Color color = Color.white;
    private Block block;

    void OnEnable()
    {
        block = new Block(GetComponent<Renderer>());
    }

    private void Start()
    {
        StartCoroutine(Move());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        block.SetColor("_Color", color);
        block.SetFloat("_Width1", width1);
        block.SetFloat("_StrokeCount1", strokeCount1);
        block.SetFloat("_Width2", width2);
        block.SetFloat("_StrokeCount2", strokeCount2);
        block.SetFloat("_Width3", width3);
        block.SetFloat("_StrokeCount3", strokeCount3);
        block.Apply();
    }

    private Vector3 current=> transform.position;
    private Vector3 target;
    public float d = 0.5f;
    public float restD = 0.5f;
    public float speed = 0.02f;

    IEnumerator Move()
    {
        yield return null;

        while (true)
        {
            if (Vector3.Distance(current, target) < d)
            {
                for (int i = 0; i < 20; i++)
                {
                    var view = new Vector2(Random.value, Random.value);
                    var wpos = Camera.main.ViewportToWorldPoint(view);
                    wpos.z = 0;

                    if (Vector3.Distance(wpos, current) > restD)
                    {
                        target = wpos;
                        break;
                    }
                }
            }

            transform.position += (target - transform.position).normalized * speed;
            yield return null;
        }

    }

}
