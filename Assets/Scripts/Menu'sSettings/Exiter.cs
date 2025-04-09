using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using System;

public class Exiter : MonoBehaviour
{
    [SerializeField] private AudioSource _AuthorsSong;
    [Range(0f, 1f)] public float _AuthorsSongVolume;

    public void Exit()
    {
        SceneManager.LoadScene(Scenes.MainMenu);
    }
    void Start()
    {
        _AuthorsSong.Play();
        
    }
    void Update()
    {
        _AuthorsSong.volume = _AuthorsSongVolume;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            enabled = false;
            _AuthorsSong.Stop();
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;

            SceneManager.LoadScene(Scenes.MainMenu);
        }
    }
}
