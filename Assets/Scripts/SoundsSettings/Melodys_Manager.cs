using UnityEngine;
using UnityEngine.SceneManagement; 

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
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
     
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    //private void OnDisable()
    //{
    //    SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    //}

    private void Start()
    {
        _SettingsAudio.loop = true;
        _SettingsAudio.Play(); 
    }

    private void Update()
    {
 
        _WholeVolumeValue = SoundsValueCommonManager.WholeSoundValue;
        _MelodyVolumeValue = SoundsValueCommonManager.MelodySoundValue;
        SetVolume(); 
    }


    private void OnActiveSceneChanged(Scene oldScene, Scene newScene)
    {
        
        if (oldScene.name == Scenes.MainMenu)
        {
            _SettingsAudio.Stop();
            //_SettingsAudio.Play();
        }

        Check(); 
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
