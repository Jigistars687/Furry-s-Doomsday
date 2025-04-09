using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GraphicsManager : MonoBehaviour
{
    [SerializeField] private Slider _FieldOfViewSlider;
    [SerializeField] private InputField _FieldOfViewTextHolder;

    private void FieldOfViewSliderOptioing()
    {
        _FieldOfViewSlider.value = PlayerPrefs.GetInt("_FieldOfViewValue");
    }
    public void FieldOfViewToString()
    {
        _FieldOfViewTextHolder.text = _FieldOfViewSlider.value.ToString();
        PlayerPrefs.SetInt("_FieldOfViewValue", Mathf.RoundToInt(_FieldOfViewSlider.value));
    }
    public void FieldOfViewHandlerToValue()
    {
        _FieldOfViewSlider.value = int.Parse(_FieldOfViewTextHolder.text);
        PlayerPrefs.SetInt("_FieldOfViewValue", Mathf.RoundToInt(_FieldOfViewSlider.value));
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _FieldOfViewTextHolder.text = "";
    }
    public void ValidateInput(string text)
    {
        string filteredText = "";
        foreach (char c in text)
        {
            if (char.IsDigit(c))
            {
                filteredText += c;
            }
        }
        
        if (text != filteredText)
        {
            _FieldOfViewTextHolder.text = filteredText;
        }
    }
    public void SaveAll()
    {
        PlayerPrefs.Save();
    }
    void Start()
    {
        FieldOfViewSliderOptioing();
        _FieldOfViewTextHolder.onValueChanged.AddListener(delegate { ValidateInput(_FieldOfViewTextHolder.text); });
    }
    void Update()
    {
    }
}
