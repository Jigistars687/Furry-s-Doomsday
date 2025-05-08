using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_manager : MonoBehaviour
{
    [SerializeField] private AudioSource _ButtonClick;
    [SerializeField] private AudioSource _ToggleClick;
    [SerializeField] private AudioClip _ToggleClickClip;
    [SerializeField] private AudioClip _ButtonClickClip;// Добавляем AudioClip для PlayOneShot

    public void ButtonClick()
    {
        _ButtonClick.PlayOneShot(_ButtonClickClip); // Передаем AudioClip в PlayOneShot
    }

    public void ToggleClick()
    {
        _ToggleClick.PlayOneShot(_ToggleClickClip); // Передаем AudioClip в PlayOneShot
    }

    void Update()
    {

    }
}
