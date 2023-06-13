using System;
using System.Collections;
using System.Collections.Generic;
using mj.gist;
using UnityEngine;

public class MicrophoneVolumeDetector : SingletonMonoBehaviour<MicrophoneVolumeDetector>
{
#if UNITY_WEBGL && !UNITY_EDITOR
    public static float GetVolume() => Microphone.devices.Length!=0 ? Microphone.volumes[0] : 0;
#else
    public static float GetVolume() => 0;
#endif

#if UNITY_WEBGL && !UNITY_EDITOR
        void Awake()
        {
            Microphone.Init();
            Microphone.QueryAudioInput();
        }
        void Update()
        {
            Microphone.Update();
        }
#endif
}
