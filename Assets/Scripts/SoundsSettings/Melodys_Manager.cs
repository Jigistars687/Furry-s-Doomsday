using UnityEngine;
using UnityEngine.SceneManagement;    // нужно для событий смены сцены

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
            DontDestroyOnLoad(gameObject);   // сохраняем объект между сценами :contentReference[oaicite:0]{index=0}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Подписываемся на событие смены активной сцены :contentReference[oaicite:1]{index=1}
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    //private void OnDisable()
    //{
    //    // Отписываемся, чтобы избежать утечек :contentReference[oaicite:2]{index=2}
    //    SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    //}

    private void Start()
    {
        _SettingsAudio.loop = true;
        _SettingsAudio.Play();            // запускаем музыку один раз :contentReference[oaicite:3]{index=3}
    }

    private void Update()
    {
        // Обновляем громкость каждый кадр — но не трогаем Play/Pause здесь
        _WholeVolumeValue = SoundsValueCommonManager.WholeSoundValue;
        _MelodyVolumeValue = SoundsValueCommonManager.MelodySoundValue;
        SetVolume();                      // нормируем и применяем громкость :contentReference[oaicite:4]{index=4}
    }

    // Вызывается только при смене активной сцены
    // Добавляем внутри вашего класса Melodys_Manager:

    // Вызывается только при смене активной сцены
    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        // Если переход из сцены главного меню — перезапустить музыку
        if (oldScene.name == Scenes.MainMenu)
        {
            _SettingsAudio.Stop();   // останавливаем текущее воспроизведение :contentReference[oaicite:0]{index=0}
            //_SettingsAudio.Play();   // запускаем заново с начала :contentReference[oaicite:1]{index=1}
        }

        Check();  // ваш существующий код паузы/запуска :contentReference[oaicite:2]{index=2}
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
