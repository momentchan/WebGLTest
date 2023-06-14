using mj.gist;
using UnityEngine;

public class BigBrush : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;
    private float ratio;
    private Block block;

    void Start()
    {
        block = new Block(GetComponentsInChildren<MeshRenderer>());    
    }

    void Update()
    {
        ratio += Time.deltaTime * speed;
        block.SetFloat("_Ratio", ratio);
        block.Apply();
    }
}
