using UnityEngine;
using UnityEngine.SceneManagement;    // ����� ��� ������� ����� �����

public class Melodys_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource _SettingsAudio;

    public static Melodys_Manager instance;
    private float _WholeVolumeValue;
    private float _MelodyVolumeValue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);   // ��������� ������ ����� ������� :contentReference[oaicite:0]{index=0}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // ������������� �� ������� ����� �������� ����� :contentReference[oaicite:1]{index=1}
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    //private void OnDisable()
    //{
    //    // ������������, ����� �������� ������ :contentReference[oaicite:2]{index=2}
    //    SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    //}

    private void Start()
    {
        _SettingsAudio.loop = true;
        _SettingsAudio.Play();            // ��������� ������ ���� ��� :contentReference[oaicite:3]{index=3}
    }

    private void Update()
    {
        // ��������� ��������� ������ ���� � �� �� ������� Play/Pause �����
        _WholeVolumeValue = SoundsValueCommonManager.WholeSoundValue;
        _MelodyVolumeValue = SoundsValueCommonManager.MelodySoundValue;
        SetVolume();                      // ��������� � ��������� ��������� :contentReference[oaicite:4]{index=4}
    }

    // ���������� ������ ��� ����� �������� �����
    // ��������� ������ ������ ������ Melodys_Manager:

    // ���������� ������ ��� ����� �������� �����
    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        // ���� ������� �� ����� �������� ���� � ������������� ������
        if (oldScene.name == Scenes.MainMenu)
        {
            _SettingsAudio.Stop();   // ������������� ������� ��������������� :contentReference[oaicite:0]{index=0}
            //_SettingsAudio.Play();   // ��������� ������ � ������ :contentReference[oaicite:1]{index=1}
        }

        Check();  // ��� ������������ ��� �����/������� :contentReference[oaicite:2]{index=2}
    }


    public void SetVolume()
    {
        //_SettingsAudio.volume = _WholeVolumeValue;
        _SettingsAudio.volume = ((_MelodyVolumeValue / 100) * (_WholeVolumeValue / 100)) * 10000;
        //Check();
    }

    private void Check()
    {
        if (PlayerPrefs.GetInt(BooleanSettings.IsInGame) == 1)
        {
            _SettingsAudio.Pause();
        }
        else if (PlayerPrefs.GetInt(BooleanSettings.IsInGame) == 0)
        {
            _SettingsAudio.UnPause();
        }
    }


}
