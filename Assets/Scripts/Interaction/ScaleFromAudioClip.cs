using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFromAudioClip : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private Vector3 minScale, maxScale;
    [SerializeField] private AudioLoudnessDetection detector;

    [SerializeField] private float loudnessSensibility = 100;
    [SerializeField] private float threshold = 0.1f;
    void Start()
    {
        
    }

    void Update()
    {
        //float loudness = detector.GetLoudnessFromAudioClip(source.timeSamples, source.clip);

        //loudness = loudness < threshold ? 0 : loudness;

        //transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);
    }
}
