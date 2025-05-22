using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingUI : MonoBehaviour
{
    [SerializeField] private Text AttackBindKeyName;
    [SerializeField] private Button AttackBindInputButton;
    
    [SerializeField] private Text ReloadBindKeyName;
    [SerializeField] private Button ReloadBindInputButton;

    [SerializeField] private Text ButtstockBlowBindKeyName;
    [SerializeField] private Button ButtstockBlowBindInputButton;

    [SerializeField] private Text ForwardBindKeyName;
    [SerializeField] private Button ForwardBindInputButton;

    [SerializeField] private Text BackwardBindKeyName;
    [SerializeField] private Button BackwardBindInputButton;

    [SerializeField] private Text GoRightBindKeyName;
    [SerializeField] private Button GoRightBindInputButton;

    [SerializeField] private Text GoLeftBindKeyName;
    [SerializeField] private Button GoLeftBindInputButton;

    private KeyCode recordedKey;

    void Start()
    {
        AddListeners();

        AttackBindKeyName.text = PlayerPrefs.GetString(KeyBinding.ATTACK_KEY, $"{KeyCode.Mouse0}");
        ReloadBindKeyName.text = PlayerPrefs.GetString(KeyBinding.RELOAD_KEY, $"{KeyCode.R}");
        ButtstockBlowBindKeyName.text = PlayerPrefs.GetString(KeyBinding.Buttstock_Blow_KEY, $"{KeyCode.V}");
        ForwardBindKeyName.text = PlayerPrefs.GetString(KeyBinding.FORWARD_KEY, $"{KeyCode.W}");
        BackwardBindKeyName.text = PlayerPrefs.GetString(KeyBinding.BACKWARD_KEY, $"{KeyCode.S}");
        GoRightBindKeyName.text = PlayerPrefs.GetString(KeyBinding.GO_RIGHT_KEY, $"{KeyCode.D}");
        GoLeftBindKeyName.text = PlayerPrefs.GetString(KeyBinding.GO_LEFT_KEY, $"{KeyCode.A}");
    }

    public void OnInputButtonClick(Button BindInputButton, Text BindKeyName, string KeyName)
    {
        BindInputButton.interactable = false;
        StartCoroutine(WaitForKeyPress(BindInputButton, BindKeyName, KeyName));
    }

    private IEnumerator WaitForKeyPress(Button BindInputButton, Text BindKeyName, string KeyName)
    {
        while (true)
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        recordedKey = keyCode;
                        break;
                    }
                }
                if (recordedKey != KeyCode.None)
                    break;
            }
            yield return null;
        }
        BindKeyName.text = recordedKey + "";
        PlayerPrefs.SetString(KeyName, $"{recordedKey}");
        BindInputButton.interactable = true;
    }

    private void AddListeners()
    {
        AttackBindInputButton.onClick.AddListener(AttackLinkToListener);
        ReloadBindInputButton.onClick.AddListener(ReloadLinkToListener);
        ButtstockBlowBindInputButton.onClick.AddListener(ButtstockBlowBindKeyLinkToListener);
        ForwardBindInputButton.onClick.AddListener(ForwardLinkToListener);
        BackwardBindInputButton.onClick.AddListener(BackwardLinkToListener);
        GoRightBindInputButton.onClick.AddListener(GoRightLinkToListener);
        GoLeftBindInputButton.onClick.AddListener(GoLeftLinkToListener);
    }
    private void AttackLinkToListener()
    {
        OnInputButtonClick(AttackBindInputButton, AttackBindKeyName, KeyBinding.ATTACK_KEY);
    }
    private void ReloadLinkToListener()
    {
        OnInputButtonClick(ReloadBindInputButton, ReloadBindKeyName, KeyBinding.RELOAD_KEY);
    }
    private void ButtstockBlowBindKeyLinkToListener()
    {
        OnInputButtonClick(ButtstockBlowBindInputButton, ButtstockBlowBindKeyName, KeyBinding.Buttstock_Blow_KEY);
    }
    private void ForwardLinkToListener()
    {
        OnInputButtonClick(ForwardBindInputButton, ForwardBindKeyName, KeyBinding.FORWARD_KEY);
    }
    private void BackwardLinkToListener()
    {
        OnInputButtonClick(BackwardBindInputButton, BackwardBindKeyName, KeyBinding.BACKWARD_KEY);
    }
    private void GoRightLinkToListener()
    {
        OnInputButtonClick(GoRightBindInputButton, GoRightBindKeyName, KeyBinding.GO_RIGHT_KEY);
    }
    private void GoLeftLinkToListener()
    {
        OnInputButtonClick(GoLeftBindInputButton, GoLeftBindKeyName, KeyBinding.GO_LEFT_KEY);
    }


}