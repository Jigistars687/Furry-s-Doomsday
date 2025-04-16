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
    private bool canTakeDamage = true;
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
        Heal();
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
        else if ((Input.GetKeyDown(KeyCode.B) & _IsCursorShowed))
        {
            _IsCursorShowed = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
    private void Heal()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            _PL_Stats.Heal(Vodka_stats.Heal);
            StartCoroutine(DamageCooldown());
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
            if (canTakeDamage)
            {
                _PL_Stats.TakeDamage(_Stats.DamagePerTick);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(1f); // Пауза на 1 секунду
        canTakeDamage = true;
    }

    private Coroutine _blinkCoroutine;

    public void RefreshHealthBar()
    {
        float healthRatio = _PL_Stats.Health / _PL_Stats.MaxHealth;
        _hBRectTransform.transform.localScale = new Vector3(_HBMaxSize * healthRatio, _hBRectTransform.transform.localScale.y, 1);
        UnityEngine.UI.Image image = _hBRectTransform.GetComponent<UnityEngine.UI.Image>();

        Color green = Color.green;
        Color brightYellow = Color.yellow;
        Color darkYellow = new Color(1f, 0.6f, 0f);
        Color red = Color.red;

        if (healthRatio > 0.25f)
        {
            if (_blinkCoroutine != null)
            {
                StopCoroutine(_blinkCoroutine);
                _blinkCoroutine = null;
            }

            if (healthRatio > 0.75f)
            {
                // При здоровье от 75% до 100%: от ярко-жёлтого к зелёному
                float t = (healthRatio - 0.75f) / 0.25f;
                image.color = Color.Lerp(brightYellow, green, t);
            }
            else if (healthRatio > 0.5f)
            {
                // При здоровье от 50% до 75%: от тёмно-жёлтого к ярко-жёлтому
                float t = (healthRatio - 0.5f) / 0.25f;
                image.color = Color.Lerp(darkYellow, brightYellow, t);
            }
            else // healthRatio в диапазоне от 0.25 до 0.5
            {
                // При здоровье от 25% до 50%: от красного к тёмно-жёлтому
                float t = (healthRatio - 0.25f) / 0.25f;
                image.color = Color.Lerp(red, darkYellow, t);
            }
        }
        else
        {
            if (_blinkCoroutine == null)
            {
                _blinkCoroutine = StartCoroutine(SmoothBlinkRedWhite());
            }
        }
    }

    private IEnumerator SmoothBlinkRedWhite()
    {
        UnityEngine.UI.Image image = _hBRectTransform.GetComponent<UnityEngine.UI.Image>();
        float t = 0f;
        float speed = 5.0f; // Коэффициент скорости смены цвета; можно настроить по вкусу

        while (true)
        {
            while (t < 1f)
            {
                image.color = Color.Lerp(Color.red, Color.white, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
            while (t > 0f)
            {
                image.color = Color.Lerp(Color.red, Color.white, t);
                t -= Time.deltaTime * speed;
                yield return null;
            }
        }
    }
}