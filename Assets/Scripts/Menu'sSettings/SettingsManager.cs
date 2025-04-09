using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UIElements.Slider;
using Toggle = UnityEngine.UI.Toggle;

public class SettingsManager : MonoBehaviour
{

    public void LoadMovementCategory()
    {

        SceneManager.LoadScene(Scenes.MenuControlsSettings);
    }
    public void LoadCommonCategory()
    {

        SceneManager.LoadScene(Scenes.MenuCommonSettings);
    }
    public void LoadSoundCategory()
    {

        SceneManager.LoadScene(Scenes.MenuSoundSettings);
    }
    public void LoadGraphycsCategory()
    {

        SceneManager.LoadScene(Scenes.MenuGraphycsSettings);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            enabled = false;
            SceneManager.LoadScene(Scenes.MainMenu);
        }

    }
}
