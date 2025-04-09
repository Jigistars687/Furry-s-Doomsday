using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameFeaturesManager : MonoBehaviour
{
    [SerializeField] private Text _fpsText;
    [SerializeField] private Camera _PlayersCamera;
    [SerializeField] private Animator _ShotGunAnim;
    [SerializeField] private Transform BulletsSpawn;
    [SerializeField] private Transform _healthBar;
    [SerializeField] private RectTransform _hBRectTransform;

    private PlayerController _player;
    private Player_stats _PL_Stats;
    private int _FpsShowed;
    private int _FieldOfView;
    private float deltaTime = 0.0f;
    private float _HBMaxSize;
    private bool _IsCursorShowed = false;
    private string _ShotGunReloadAnimText;
    private string _ShotGunShootAnimText;
    private string _ShotGunbuttstockPunchAnimText;


    void Awake()
    {
        _PL_Stats = new Player_stats();
        UnityEngine.Cursor.visible = false;
        _FpsShowed = PlayerPrefs.GetInt("_IsFpsShouldShows");
        _FieldOfView = PlayerPrefs.GetInt("_FieldOfViewValue");
        _PlayersCamera.fieldOfView = _FieldOfView;
        _HBMaxSize = _hBRectTransform.transform.localScale.x;
        _PL_Stats.HealthChanger += RefreshHealthBar;
 //       _PL_Stats.HealthChanger += IsDead;
 //       _PL_Stats.LoadReferences();
    }

    void Start()
    {
        _player = GetComponent<PlayerController>();
        _ShotGunReloadAnimText = "IsReload";
        _ShotGunShootAnimText = "Shoot";
        _ShotGunbuttstockPunchAnimText = "ButtstockPunch";
    }

    void Update()
    {
        CursorShowing();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _player.enabled = false;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            PlayerPrefs.SetInt(BooleanSettings.IsInGame, 0);
            SceneManager.LoadScene(Scenes.GameCommonSettings);
        }
        if (Input.GetKeyDown(BindingKeysManager.Reload_Key_KEYCODE))
        {
            _ShotGunAnim.SetTrigger(_ShotGunReloadAnimText);
        }
        if (Input.GetKeyDown(BindingKeysManager.Attack_Key_KEYCODE))
        {
            _ShotGunAnim.SetTrigger(_ShotGunShootAnimText);
        }
        if (Input.GetKeyDown(BindingKeysManager.Buttstock_Blow_KEYCODE))
        {
            _ShotGunAnim.SetTrigger(_ShotGunbuttstockPunchAnimText);
        }
        if (_FpsShowed == 1)
        {
            ShowFps();
        }
    }
    private void CursorShowing()
    {
        if (Input.GetKeyDown(KeyCode.B) & !_IsCursorShowed)
        {
            _IsCursorShowed = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else if((Input.GetKeyDown(KeyCode.B) & _IsCursorShowed))
        {
            _IsCursorShowed = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
    private void ShowFps()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / Time.unscaledDeltaTime;
        _fpsText.text = Mathf.Ceil(fps).ToString() + " FPS";
        if (Mathf.Ceil(fps) >= 60)
        {
            _fpsText.color = Color.green;
        }
        if (Mathf.Ceil(fps) < 60 & Mathf.Ceil(fps) > 20)
        {
            _fpsText.color = Color.yellow;
        }
        if (Mathf.Ceil(fps) < 20)
        {
            _fpsText.color = Color.red;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy_Stats>(out var _Stats))
        {
            _PL_Stats.TakeDamage(_Stats.DamagePerTick);
        }
    }
    public void RefreshHealthBar()
    {
        _hBRectTransform.transform.localScale = new Vector3(_HBMaxSize * (_PL_Stats.Health / _PL_Stats.MaxHealth), _hBRectTransform.transform.localScale.y, 1);
    }
}
