using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsValueCommonManager : MonoBehaviour
{
    public static string WholeVolumeValueText = "_WholeVolumeValue";
    public static string MelodyVolumeValueText = "_MelodyVolumeValue";
    public static string SFXVolumeValueText = "_SFXVolumeValue";

    public static float WholeSoundValue;
    public static float MelodySoundValue;
    public static float SFXSoundValue;

    void Update()
    {

        WholeSoundValue = PlayerPrefs.GetFloat(SoundsValueCommonManager.WholeVolumeValueText);
        MelodySoundValue = PlayerPrefs.GetFloat(SoundsValueCommonManager.MelodyVolumeValueText);
        SFXSoundValue = PlayerPrefs.GetFloat(SoundsValueCommonManager.SFXVolumeValueText);
    }
}
