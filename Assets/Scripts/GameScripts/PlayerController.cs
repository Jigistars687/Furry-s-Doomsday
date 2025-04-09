using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [Range(1, 10)] public float walkingSpeed = 1;
    [Range(1, 10)] public float turnSpeed = 1;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(BindingKeysManager.Forward_Key_KEYCODE))
        {
            movement += transform.forward * walkingSpeed;
        }
        if (Input.GetKey(BindingKeysManager.Backward_Key_KEYCODE))
        {
            movement -= transform.forward * walkingSpeed;
        }
        if (Input.GetKey(BindingKeysManager.GoLeft_Key_KEYCODE))
        {
            movement -= transform.right * walkingSpeed;
        }
        if (Input.GetKey(BindingKeysManager.GoRight_Key_KEYCODE))
        {
            movement += transform.right * walkingSpeed;
        }

        movement = movement.normalized * walkingSpeed;
        characterController.Move(movement * Time.deltaTime);

        RotateAxisX();
    }

    private void InversionRotateAxisX()
    {
        float rotateValue = Input.GetAxis("Mouse X") * turnSpeed;
        transform.Rotate(0, -rotateValue, 0);
    }
    private void NotInversionRotateAxisX()
    {
        float rotateValue = Input.GetAxis("Mouse X") * turnSpeed;
        transform.Rotate(0, rotateValue, 0);
    }
    private void RotateAxisX()
    {
        if (PlayerPrefs.GetInt(BooleanSettings.IsInversionX) == 1) InversionRotateAxisX();
        else NotInversionRotateAxisX();
    }
}
