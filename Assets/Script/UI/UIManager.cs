using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI killsText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI reserveAmmoText;
    public TextMeshProUGUI healthPacksText;
    public Image healthBar;
    public Image healthBarBackground;

    [Header("Crosshair")]
    public GameObject crosshair;

    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject shopMenu;
    public GameObject mobileControlsRoot;

    [Header("Audio Settings")]
    [SerializeField] Button pauseSettingsButton;
    [SerializeField] AudioSettingsUI audioSettingsUI;

    WeaponSwitcher weaponSwitcher;

    [Header("Game Over")]
    public TextMeshProUGUI gameOverTitleText;
    public TextMeshProUGUI finalKillsText;
    public TextMeshProUGUI finalWaveText;

    void OnEnable()
    {
        if (pauseSettingsButton != null)
            pauseSettingsButton.onClick.AddListener(OpenAudioSettings);
    }

    void OnDisable()
    {
        if (pauseSettingsButton != null)
            pauseSettingsButton.onClick.RemoveListener(OpenAudioSettings);
    }

    void Start()
    {
        // Ẩn các menu
        if (pauseMenu) pauseMenu.SetActive(false);
        if (gameOverMenu) gameOverMenu.SetActive(false);
        if (shopMenu) shopMenu.SetActive(false);
        if (mobileControlsRoot) mobileControlsRoot.SetActive(true);
        weaponSwitcher = FindAnyObjectByType<WeaponSwitcher>();

        // Hiện crosshair
        if (crosshair) crosshair.SetActive(true);

        GameManager.SetGameplayCursor(true);
    }

    void Update()
    {
        UpdateAmmoDisplay();
        UpdateWaveAndSurvivalTime();
    }

    public void UpdateMoney(int money)
    {
        if (moneyText)
            moneyText.text = "$" + money.ToString();
    }

    public void UpdateKills(int kills)
    {
        if (killsText)
            killsText.text = "Kills: " + kills.ToString();
    }

    public void UpdateWave(int wave)
    {
        if (waveText)
            waveText.text = "WAVE: " + wave + " / " + GameManager.TotalWaves;
    }

    void UpdateWaveAndSurvivalTime()
    {
        GameManager gameManager = GameManager.Instance;
        if (waveText == null || gameManager == null)
            return;

        waveText.text = "WAVE: " + gameManager.currentWave + " / " + GameManager.TotalWaves +
            "   TIME: " + FormatTime(gameManager.SurvivalTime);
    }

    static string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.FloorToInt(seconds);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int remainingSeconds = totalSeconds % 60;
        return hours > 0
            ? string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, remainingSeconds)
            : string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBar && maxHealth > 0)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            healthBar.fillAmount = healthPercent;

            // Đổi màu health bar theo % máu
            if (healthPercent > 0.5f)
                healthBar.color = Color.green;
            else if (healthPercent > 0.25f)
                healthBar.color = Color.yellow;
            else
                healthBar.color = Color.red;
        }
    }

    void UpdateAmmoDisplay()
    {
        if (reserveAmmoText)
            reserveAmmoText.text = "RESERVE: " + GameManager.GetReserveAmmo();
        if (healthPacksText)
            healthPacksText.text = "HEAL x" + GameManager.GetHealthPackCount();

        // Lấy ammo từ gun hiện tại
        if (weaponSwitcher != null)
        {
            Gun currentGun = weaponSwitcher.GetCurrentWeapon();
            if (currentGun != null && ammoText != null)
            {
                ammoText.text = currentGun.currentAmmo + " / " + currentGun.maxAmmo +
                    (currentGun.HasInfiniteAmmo ? " (∞)" : "");
            }
        }
    }

    public void ShowPauseMenu()
    {
        if (pauseMenu)
        {
            pauseMenu.SetActive(true);
            if (crosshair) crosshair.SetActive(false);
            if (mobileControlsRoot) mobileControlsRoot.SetActive(false);
        }
    }

    public void HidePauseMenu()
    {
        if (pauseMenu)
        {
            pauseMenu.SetActive(false);
            if (crosshair) crosshair.SetActive(true);
            if (mobileControlsRoot) mobileControlsRoot.SetActive(true);
        }
    }

    public void OpenAudioSettings()
    {
        if (audioSettingsUI != null)
            audioSettingsUI.Open();
        else
            Debug.LogError("Assign the AudioSettingsUI component to UIManager.", this);
    }

    public void ShowGameOver(bool completedAllWaves = false)
    {
        if (gameOverMenu)
        {
            gameOverMenu.SetActive(true);
            if (crosshair) crosshair.SetActive(false);
            if (mobileControlsRoot) mobileControlsRoot.SetActive(false);

            GameManager gm = GameManager.Instance;
            if (gm != null)
            {
                if (gameOverTitleText)
                    gameOverTitleText.text = completedAllWaves ? "VICTORY!" : "GAME OVER";
                if (finalKillsText)
                    finalKillsText.text = "Total Kills: " + gm.totalKills +
                        "\nSurvival Time: " + FormatTime(gm.SurvivalTime);
                if (finalWaveText)
                    finalWaveText.text = completedAllWaves
                        ? "Waves Cleared: " + GameManager.TotalWaves + " / " + GameManager.TotalWaves
                        : "Wave Reached: " + gm.currentWave + " / " + GameManager.TotalWaves;
            }

            GameManager.SetGameplayCursor(false);
        }
    }

    public void ToggleShop()
    {
        if (shopMenu)
        {
            bool isActive = !shopMenu.activeSelf;
            shopMenu.SetActive(isActive);

            if (isActive)
            {
                ShopUI shopUI = shopMenu.GetComponent<ShopUI>();
                if (shopUI != null)
                    shopUI.RefreshButtonStates();

                Time.timeScale = 0f;
                GameManager.SetGameplayCursor(false);
                if (crosshair) crosshair.SetActive(false);
                if (mobileControlsRoot) mobileControlsRoot.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
                GameManager.SetGameplayCursor(true);
                if (crosshair) crosshair.SetActive(true);
                if (mobileControlsRoot) mobileControlsRoot.SetActive(true);
            }
        }
    }
}
