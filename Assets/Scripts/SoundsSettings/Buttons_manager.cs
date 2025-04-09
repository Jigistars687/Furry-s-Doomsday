using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_manager : MonoBehaviour
{
    [SerializeField] private AudioSource _ButtonClick;
    [SerializeField] private AudioSource _ToggleClick;
    public void ButtonClick()
    {
        _ButtonClick.Play();
    }
    public void ToggleClick()
    {
        _ToggleClick.Play();
    }

    void Update()
    {
        
    }
}
