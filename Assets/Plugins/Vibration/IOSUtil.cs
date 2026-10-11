using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class IOSUtil : MonoBehaviour
{
    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport ("__Internal")]
        static extern void _playSystemSound(int n);
    #endif

    public static void PlaySystemSound(int n) //引数にIDを渡す
    {
    #if UNITY_IOS && !UNITY_EDITOR
            _playSystemSound(n);
    #endif
    }
}
