using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundCategoryManager : MonoBehaviour
{
    [SerializeField] private Slider _WholeVolumeValueSlider;
    [SerializeField] private InputField _WholeVolumeValueInputField;

    [SerializeField] private Slider _MelodyVolumeValueSlider;
    [SerializeField] private InputField _MelodyVolumeValueInputField;

    [SerializeField] private Slider _SFXVolumeValueSlider;
    [SerializeField] private InputField _SFXVolumeValueInputField;

    //   [SerializeField] private Text _TESTTEXT;

    void Start()
    {
        WholeVolumeValueSliderOptioing();
        MelodyVolumeValueSliderOptioing();
        SFXVolumeValueSliderOptioing();

        _WholeVolumeValueInputField.onValueChanged.AddListener(delegate { ValidateInput(_WholeVolumeValueInputField.text); });
        _WholeVolumeValueInputField.onValueChanged.AddListener(delegate { ValidateInput(_MelodyVolumeValueInputField.text); });
        _WholeVolumeValueInputField.onValueChanged.AddListener(delegate { ValidateInput(_SFXVolumeValueInputField.text); });
    }
    public void WholeVolumeValueToString()
    {
        _WholeVolumeValueInputField.text = _WholeVolumeValueSlider.value.ToString();
        PlayerPrefs.SetFloat(SoundsValueCommonManager.WholeVolumeValueText, (_WholeVolumeValueSlider.value) / 100);
    }
    private void WholeVolumeValueSliderOptioing()
    {
        _WholeVolumeValueSlider.value = PlayerPrefs.GetFloat(SoundsValueCommonManager.WholeVolumeValueText) *100;
    }
    public void WholeVolumeValueInputFieldToValue()
    {
        _WholeVolumeValueSlider.value = float.Parse(_WholeVolumeValueInputField.text);
        PlayerPrefs.SetFloat(SoundsValueCommonManager.WholeVolumeValueText, (_WholeVolumeValueSlider.value) / 100);
    }



    public void MelodyVolumeValueToString()
    {
        _MelodyVolumeValueInputField.text = _MelodyVolumeValueSlider.value.ToString();
        PlayerPrefs.SetFloat(SoundsValueCommonManager.MelodyVolumeValueText, (_MelodyVolumeValueSlider.value) / 100);
    }
    private void MelodyVolumeValueSliderOptioing()
    {
        _MelodyVolumeValueSlider.value = PlayerPrefs.GetFloat(SoundsValueCommonManager.MelodyVolumeValueText) * 100;
    }
    public void MelodyVolumeValueInputFieldToValue()
    {
        _MelodyVolumeValueSlider.value = float.Parse(_MelodyVolumeValueInputField.text);
        PlayerPrefs.SetFloat(SoundsValueCommonManager.MelodyVolumeValueText, (_MelodyVolumeValueSlider.value) / 100);
    }



    public void SFXVolumeValueToString()
    {
        _SFXVolumeValueInputField.text = _SFXVolumeValueSlider.value.ToString();
        PlayerPrefs.SetFloat(SoundsValueCommonManager.SFXVolumeValueText, (_SFXVolumeValueSlider.value) / 100);
    }
    private void SFXVolumeValueSliderOptioing()
    {
        _SFXVolumeValueSlider.value = PlayerPrefs.GetFloat(SoundsValueCommonManager.SFXVolumeValueText) * 100;
    }
    public void SFXVolumeValueInputFieldToValue()
    {
        _SFXVolumeValueSlider.value = float.Parse(_SFXVolumeValueInputField.text);
        PlayerPrefs.SetFloat(SoundsValueCommonManager.SFXVolumeValueText, (_SFXVolumeValueSlider.value) / 100);
    }

    //    public void TESTED()
    //    {
    //        _TESTTEXT.text = Melodys_Manager.SET;
    //    }

    private void Update()
    {
//        TESTED();
    }
    public void ValidateInput(string text)
    {
        string _filteredText = "";
        foreach (char c in text)
        {
            if (char.IsDigit(c))
            {
                _filteredText += c;
            }
        }

        if (text != _filteredText)
        {
            _WholeVolumeValueInputField.text = _filteredText;
        }
    }

}
