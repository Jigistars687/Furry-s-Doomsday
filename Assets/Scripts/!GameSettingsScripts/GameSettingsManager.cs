using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameSettingsManager : MonoBehaviour
{
    public void LoadMovementCategory()
    {
        SceneManager.LoadScene(Scenes.GameControlsSettings);
    }
    public void LoadCommonCategory()
    {
        SceneManager.LoadScene(Scenes.GameCommonSettings);
    }
    public void LoadSoundCategory()
    {
        SceneManager.LoadScene(Scenes.GameSoundSettings);
    }
    public void LoadGraphycsCategory()
    {
        SceneManager.LoadScene(Scenes.GameGraphycsSettings);
    }
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(Scenes.MainMenu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enabled = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            PlayerPrefs.SetInt(BooleanSettings.IsInGame, 1);
            SceneManager.LoadScene(Scenes.NewGame);
        }

    }
}
