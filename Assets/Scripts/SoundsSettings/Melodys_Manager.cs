using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Melodys_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource _SettingsAudio;

    public static Melodys_Manager instance;
    private float _WholeVolumeValue;
    private float _MelodyVolumeValue;
//   public static string SET;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume()
    {
//        _SettingsAudio.volume = _WholeVolumeValue;
        _SettingsAudio.volume = ((_MelodyVolumeValue / 100) * (_WholeVolumeValue/100)) * 10000;
    }
    private void Update()
    {
        _WholeVolumeValue = SoundsValueCommonManager.WholeSoundValue;
        _MelodyVolumeValue = SoundsValueCommonManager.MelodySoundValue;
        SetVolume();
        Check();
//        SET = _SettingsAudio.volume.ToString();
    }
    private void Check()
    {
        if (PlayerPrefs.GetInt(BooleanSettings.IsInGame) == 1)
        {
            _SettingsAudio.Stop();
            Debug.Log("в игре");
        }
        else if (PlayerPrefs.GetInt(BooleanSettings.IsInGame) == 0)
        {
            _SettingsAudio.Play();
            Debug.Log(" не в игре");
        }
    }
}

