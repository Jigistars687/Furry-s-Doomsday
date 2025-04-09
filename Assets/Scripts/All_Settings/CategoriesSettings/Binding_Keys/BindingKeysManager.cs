using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindingKeysManager : MonoBehaviour
{
    private string Attack_Key_STR;
    private string Reload_Key_STR;
    private string Buttstock_Blow_Key_STR;
    private string Forward_Key_STR;
    private string Backward_Key_STR;
    private string GoRight_Key_STR;
    private string GoLeft_Key_STR;

    public static KeyCode Attack_Key_KEYCODE;
    public static KeyCode Reload_Key_KEYCODE;
    public static KeyCode Buttstock_Blow_KEYCODE;
    public static KeyCode Forward_Key_KEYCODE;
    public static KeyCode Backward_Key_KEYCODE;
    public static KeyCode GoRight_Key_KEYCODE;
    public static KeyCode GoLeft_Key_KEYCODE;

    private void Start()
    {
        Attack_Key_STR = PlayerPrefs.GetString(KeyBinding.ATTACK_KEY);
        Reload_Key_STR = PlayerPrefs.GetString(KeyBinding.RELOAD_KEY);
        Buttstock_Blow_Key_STR = PlayerPrefs.GetString(KeyBinding.Buttstock_Blow_KEY);
        Forward_Key_STR = PlayerPrefs.GetString(KeyBinding.FORWARD_KEY);
        Backward_Key_STR = PlayerPrefs.GetString(KeyBinding.BACKWARD_KEY);
        GoRight_Key_STR = PlayerPrefs.GetString(KeyBinding.GO_RIGHT_KEY);
        GoLeft_Key_STR = PlayerPrefs.GetString(KeyBinding.GO_LEFT_KEY);

        Attack_Key_KEYCODE = GetKeysFromSTR(Attack_Key_KEYCODE, Attack_Key_STR, KeyCode.Mouse0);
        Reload_Key_KEYCODE = GetKeysFromSTR(Reload_Key_KEYCODE, Reload_Key_STR, KeyCode.R);
        Buttstock_Blow_KEYCODE = GetKeysFromSTR(Buttstock_Blow_KEYCODE, Buttstock_Blow_Key_STR, KeyCode.V);
        Forward_Key_KEYCODE = GetKeysFromSTR(Forward_Key_KEYCODE, Forward_Key_STR, KeyCode.W);
        Backward_Key_KEYCODE = GetKeysFromSTR(Backward_Key_KEYCODE, Backward_Key_STR, KeyCode.S);
        GoRight_Key_KEYCODE = GetKeysFromSTR(GoRight_Key_KEYCODE, GoRight_Key_STR, KeyCode.D);
        GoLeft_Key_KEYCODE = GetKeysFromSTR(GoLeft_Key_KEYCODE, GoLeft_Key_STR, KeyCode.A);
    } 

    private KeyCode GetKeysFromSTR(KeyCode Key, string Key_STR, KeyCode DefaultKeyCode)
    {
        if (System.Enum.TryParse(Key_STR, out KeyCode keycode))
        {
            Key = keycode;
        }
        else
        {
            Key = DefaultKeyCode;
        }

        return Key;
    }
}
