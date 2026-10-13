using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] GameObject settingsPanel;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Button closeButton;

    void Awake()
    {
        if (settingsPanel == null)
            Debug.LogError("Assign the Audio Settings panel.", this);
        if (musicSlider == null)
            Debug.LogError("Assign the Music slider.", this);
        if (sfxSlider == null)
            Debug.LogError("Assign the SFX slider.", this);
        if (closeButton == null)
            Debug.LogError("Assign the Audio Settings close button.", this);

        if (musicSlider != null)
            musicSlider.SetValueWithoutNotify(AudioManager.GetMusicVolume());
        if (sfxSlider != null)
            sfxSlider.SetValueWithoutNotify(AudioManager.GetSFXVolume());

        AudioManager.ApplySavedSettings();

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void OnEnable()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        if (closeButton != null)
            closeButton.onClick.AddListener(Close);
    }

    void OnDisable()
    {
        if (musicSlider != null)
            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
        if (closeButton != null)
            closeButton.onClick.RemoveListener(Close);
    }

    public void Open()
    {
        if (settingsPanel == null)
        {
            Debug.LogError("Cannot open Audio Settings: no settings panel is assigned.", this);
            return;
        }

        settingsPanel.SetActive(true);
        settingsPanel.transform.SetAsLastSibling();
    }

    public void Close()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    void SetMusicVolume(float volume)
    {
        AudioManager.SetMusicSetting(volume);
    }

    void SetSFXVolume(float volume)
    {
        AudioManager.SetSFXSetting(volume);
    }
}
