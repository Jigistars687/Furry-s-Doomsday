using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CommonSettings : MonoBehaviour
{
    [SerializeField] private Toggle _FpsToggle;
    private int _IsActiveFpsToggle;
    private void Start()
    {
        _IsActiveFpsToggle = PlayerPrefs.GetInt("_SavedFpsToggle");
        CheckFpsToogle();
    }
    public void SaveAll()
    {
        PlayerPrefs.Save();
    }
    private void CheckFpsToogle()
    {
        if (_IsActiveFpsToggle == 1)
        {
            _FpsToggle.isOn = true;
        }
        else
        {
            _FpsToggle.isOn = false;
        }
    }

    public void ChangeToggleCondition()
    {
        if (_FpsToggle.isOn)
        {
            PlayerPrefs.SetInt("_SavedFpsToggle", 1);
            PlayerPrefs.SetInt("_IsFpsShouldShows", 1);
        }
        else
        {
            PlayerPrefs.SetInt("_SavedFpsToggle", 0);
            PlayerPrefs.SetInt("_IsFpsShouldShows", 0);
        }
    }
    private void Update()
    {
    }
}
