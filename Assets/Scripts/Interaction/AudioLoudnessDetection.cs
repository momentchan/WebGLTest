using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    [SerializeField] private float smoothness = 0.5f;
    private AudioClip microphoneClip;
    private float prevVolume = 0;
    private int sampleWindow = 60;

#if UNITY_WEBGL && !UNITY_EDITOR
        void Awake()
        {
            Microphone.Init();
            Microphone.QueryAudioInput();
        }
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        void Update()
        {
            Microphone.Update();
        }
#endif

    private void Start()
    {
        //var microphone = Microphone.devices[0];
        //microphoneClip = Microphone.Start(microphone, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        //return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
        float[] volumes = Microphone.volumes;
        return volumes[0];


#else
        return 0;
#endif

    }

    void OnGUI()
    {
        GUILayout.BeginVertical(GUILayout.Height(Screen.height));
        GUILayout.FlexibleSpace();

        string[] devices = Microphone.devices;

#if UNITY_WEBGL && !UNITY_EDITOR
            float[] volumes = Microphone.volumes;
#endif

        GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
        GUILayout.FlexibleSpace();
        GUILayout.Label(string.Format("Microphone count={0}", devices.Length));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        for (int index = 0; index < devices.Length; ++index)
        {
            string deviceName = devices[index];
            if (deviceName == null)
            {
                deviceName = string.Empty;
            }

            GUILayout.BeginHorizontal(GUILayout.Width(Screen.width));
            GUILayout.FlexibleSpace();
#if UNITY_WEBGL && !UNITY_EDITOR
                GUILayout.Label(string.Format("Device Name={0} Volume={1}", deviceName, volumes[index]));
#else
            GUILayout.Label(string.Format("Device Name={0}", deviceName));
#endif
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;
        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);
        float totalLoudness = 0;

        for (var i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(waveData[i]);

        }

        return totalLoudness / sampleWindow;
    }
}
