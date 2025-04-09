using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_manager : MonoBehaviour
{
    [SerializeField] private AudioSource _ButtonClick;
    [SerializeField] private AudioSource _ToogleClick;

    private float _WholeVolumeValue;
    private float _SFXVolumeValue;
    public void SetVolume()
    {
        _ButtonClick.volume = ((_SFXVolumeValue / 100) * (_WholeVolumeValue / 100)) * 10000;
        _ToogleClick.volume = ((_SFXVolumeValue / 100) * (_WholeVolumeValue / 100)) * 10000;
    }

    void Start()
    {
        
    }
    void Update()
    {
        _WholeVolumeValue = SoundsValueCommonManager.WholeSoundValue;
        _SFXVolumeValue = SoundsValueCommonManager.SFXSoundValue;
        SetVolume();
    }
}
