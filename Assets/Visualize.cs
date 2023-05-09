using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualize : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private List<GameObject> prefabs = new List<GameObject>();
    int count = 64;
    public float strength = 20;
    void Start()
    {
        for (var i = 0; i < count; i++)
        {
            var go = Instantiate(prefab, transform);
            go.transform.localPosition = (i - count * 0.5f) * Vector3.right * 0.1f;
            prefabs.Add(go);
        }
    }
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(50, 50, 200, 100));
        strength = GUILayout.HorizontalSlider(strength, 0, 100);
        GUILayout.EndArea();
    }
    void Update()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        for (var i = 0; i < count; i++)
        {
            var s = prefabs[i].transform.localScale;
            s.y = 0.2f + strength *¡@Microphone.GetData(1024 / count * i);
            prefabs[i].transform.localScale = s;
        }
#endif

    }
}
