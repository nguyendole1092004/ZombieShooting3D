using UnityEngine;

public class AudioManager : MonoBehaviour
{
    const string MusicVolumePreferenceKey = "MusicVolume";
    const string SFXVolumePreferenceKey = "SFXVolume";

    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Background Music")]
    public AudioClip menuMusic;
    public AudioClip gameplayMusic;

    [Header("Sound Effects")]
    public AudioClip buttonClick;
    public AudioClip zombieGrowl;
    public AudioClip playerHurt;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ApplySavedSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic(gameplayMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource != null && clip != null)
        {
            musicSource.ignoreListenerVolume = true;
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.volume = GetMusicVolume();
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void SetMusicVolume(float volume)
    {
        SetMusicSetting(volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetSFXSetting(volume);
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MusicVolumePreferenceKey, 1f);
    }

    public static float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFXVolumePreferenceKey, 1f);
    }

    public static void SetMusicSetting(float volume)
    {
        PlayerPrefs.SetFloat(MusicVolumePreferenceKey, Mathf.Clamp01(volume));
        PlayerPrefs.Save();

        if (Instance != null && Instance.musicSource != null)
            Instance.musicSource.volume = GetMusicVolume();
    }

    public static void SetSFXSetting(float volume)
    {
        PlayerPrefs.SetFloat(SFXVolumePreferenceKey, Mathf.Clamp01(volume));
        PlayerPrefs.Save();
        AudioListener.volume = GetSFXVolume();
    }

    public static void ApplySavedSettings()
    {
        AudioListener.volume = GetSFXVolume();

        if (Instance != null && Instance.musicSource != null)
        {
            Instance.musicSource.ignoreListenerVolume = true;
            Instance.musicSource.volume = GetMusicVolume();
        }
    }
}
