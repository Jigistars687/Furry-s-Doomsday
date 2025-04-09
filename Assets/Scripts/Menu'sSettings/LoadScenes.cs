using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.SetInt(BooleanSettings.IsInGame, 0);
    }
    public void LoadNewGameScene()
    {
        PlayerPrefs.SetInt(BooleanSettings.IsInGame, 1);
        SceneManager.LoadScene(Scenes.NewGame);
    }
    public void LoadResumeGameScene()
    {
        PlayerPrefs.SetInt(BooleanSettings.IsInGame, 1);
        SceneManager.LoadScene(Scenes.ResumeGame);
    }
    public void LoadGameSettingsScene()
    {

        SceneManager.LoadScene(Scenes.GameCommonSettings);
    }
    public void LoadMenuSettingsScene()
    {

        SceneManager.LoadScene(Scenes.MenuCommonSettings);
    }
    public void LoadAuthorsScene()
    {

        SceneManager.LoadScene(Scenes.Authors);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
