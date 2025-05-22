using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlCategories : MonoBehaviour
{
    [SerializeField] private Toggle InversionXToggle;
    void Start()
    {
        SetInversionXToggle();
    }

    void Update()
    {
        
    }

    public void IsInversionXToggle()
    {
        if (InversionXToggle.isOn)
        {
            PlayerPrefs.SetInt(BooleanSettings.IsInversionX, 1);
        }
        else PlayerPrefs.SetInt(BooleanSettings.IsInversionX, 0);
    }
    private void SetInversionXToggle()
    {
        if (PlayerPrefs.GetInt(BooleanSettings.IsInversionX) == 1) InversionXToggle.isOn = true;

        else InversionXToggle.isOn = false;
    }
}
