using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Short...短い振動
//DoubleShort ...2連続の短い振動
//Norma ...やや長め振動
public enum VibrationType
{
    Short, Normal, DoubleShort
}

public class VibrationPlayer : MonoBehaviour
{
    public static void Vibrate(VibrationType vib_type)
    {
        switch (vib_type)
        {
            case VibrationType.Short:
#if UNITY_IOS && !UNITY_EDITOR
        IOSUtil.PlaySystemSound(1519);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtil.Vibrate(3);
#endif
            break;
            case VibrationType.DoubleShort:
#if UNITY_IOS && !UNITY_EDITOR
        IOSUtil.PlaySystemSound(1102);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtil.Vibrate(6);
#endif
            break;
            case VibrationType.Normal:
#if UNITY_IOS && !UNITY_EDITOR
        IOSUtil.PlaySystemSound(1520);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidUtil.Vibrate(5);
#endif                
                break;
        }
    }
}
