using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    const string GameplayScene = "testscene";

    [Header("Menu")]
    [SerializeField] GameObject mainMenuPanel;

    [Header("Navigation Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button armoryButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;
    [SerializeField] TMP_Text balanceText;

    [Header("Existing Shop Menu")]
    [SerializeField] ShopUI armoryShop;
    [SerializeField] AudioSettingsUI audioSettingsUI;

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ValidateReferences();

        if (armoryShop != null)
            armoryShop.ConfigureForMainMenu(this);
        RefreshBalanceDisplay();
    }

    void OnEnable()
    {
        AddListener(playButton, StartGame);
        AddListener(armoryButton, ShowArmory);
        AddListener(settingsButton, OpenAudioSettings);
        AddListener(quitButton, QuitGame);
    }

    void OnDisable()
    {
        RemoveListener(playButton, StartGame);
        RemoveListener(armoryButton, ShowArmory);
        RemoveListener(settingsButton, OpenAudioSettings);
        RemoveListener(quitButton, QuitGame);
    }

    void ValidateReferences()
    {
        RequireReference(mainMenuPanel, nameof(mainMenuPanel));
        RequireReference(playButton, nameof(playButton));
        RequireReference(armoryButton, nameof(armoryButton));
        RequireReference(settingsButton, nameof(settingsButton));
        RequireReference(quitButton, nameof(quitButton));
        RequireReference(armoryShop, nameof(armoryShop));
        if (armoryShop != null && armoryShop.weaponShop == null)
            Debug.LogError("Assign the existing WeaponShop component to the Armory ShopUI.", armoryShop);
    }

    void RequireReference(Object reference, string fieldName)
    {
        if (reference == null)
            Debug.LogError("MainMenuController is missing its Inspector reference: " + fieldName, this);
    }

    void AddListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.AddListener(action);
    }

    void RemoveListener(Button button, UnityEngine.Events.UnityAction action)
    {
        if (button != null)
            button.onClick.RemoveListener(action);
    }

    public void StartGame()
    {
        if (!Application.CanStreamedLevelBeLoaded(GameplayScene))
        {
            Debug.LogError("Cannot start game: add Assets/Scenes/testscene.unity to Build Settings.", this);
            return;
        }

        SceneManager.LoadScene(GameplayScene);
    }

    public void ShowArmory()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);
        if (armoryShop != null)
        {
            armoryShop.gameObject.SetActive(true);
            armoryShop.RefreshButtonStates();
        }
    }

    public void ShowMainMenu()
    {
        if (armoryShop != null)
            armoryShop.gameObject.SetActive(false);
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        RefreshBalanceDisplay();
    }

    public void OpenAudioSettings()
    {
        if (audioSettingsUI != null)
            audioSettingsUI.Open();
        else
            Debug.LogError("Assign the AudioSettingsUI component to MainMenuController.", this);
    }

    public void RefreshBalanceDisplay()
    {
        if (balanceText != null)
            balanceText.text = "CREDITS  $" + GameManager.GetSavedMoney(500);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
