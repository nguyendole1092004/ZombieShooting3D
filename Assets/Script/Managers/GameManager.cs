using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    const string MoneyPreferenceKey = "PlayerMoney";
    const string WeaponPreferencePrefix = "WeaponUnlocked_";
    const string ReserveAmmoPreferenceKey = "ReserveAmmo";
    const string HealthPackPreferenceKey = "HealthPacks";
    const string MainMenuScene = "MainMenu";
    const int StartingReserveAmmo = 90;
    const int StartingHealthPacks = 1;
    public const int HealthPackHealAmount = 50;
    public const int TotalWaves = 10;

    public static GameManager Instance;

    [Header("Game State")]
    public bool isGameActive = true;
    public bool isPaused = false;

    [Header("Player Stats")]
    public int playerMoney = 0;
    public int totalKills = 0;
    public int currentWave = 1;
    public float SurvivalTime => Time.timeSinceLevelLoad;

    [Header("UI Reference")]
    private UIManager uiManager;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        playerMoney = GetSavedMoney(playerMoney);
        if (!PlayerPrefs.HasKey(MoneyPreferenceKey))
            SetSavedMoney(playerMoney);
        if (!PlayerPrefs.HasKey(ReserveAmmoPreferenceKey))
            SetReserveAmmo(StartingReserveAmmo);
        if (!PlayerPrefs.HasKey(HealthPackPreferenceKey))
            SetHealthPackCount(StartingHealthPacks);
    }

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        Time.timeScale = 1f;
        isGameActive = true;

        // Khởi tạo UI
        if (uiManager != null)
        {
            uiManager.UpdateMoney(playerMoney);
            uiManager.UpdateKills(totalKills);
            uiManager.UpdateWave(currentWave);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public static void SetGameplayCursor(bool locked)
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
#else
        bool lockCursor = locked && !Application.isMobilePlatform;
        Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !lockCursor && !Application.isMobilePlatform;
#endif
    }

    void Update()
    {
        // Pause game bằng ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot add a negative amount of money.", this);
            return;
        }

        SetSavedMoney(playerMoney + amount);
        Debug.Log("Money: " + playerMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot spend a negative amount of money.", this);
            return false;
        }

        if (playerMoney < amount)
            return false;

        SetSavedMoney(playerMoney - amount);
        return true;
    }

    public void AddKill()
    {
        totalKills++;
        if (uiManager != null)
            uiManager.UpdateKills(totalKills);
    }

    public void UpdateWave(int wave)
    {
        currentWave = wave;
        if (uiManager != null)
            uiManager.UpdateWave(currentWave);
    }

    public void PlayerDied()
    {
        isGameActive = false;
        Debug.Log("Game Over!");

        if (uiManager != null)
            uiManager.ShowGameOver(false);

        Time.timeScale = 0f;
    }

    public void CompleteGame()
    {
        if (!isGameActive)
            return;

        isGameActive = false;
        isPaused = false;
        Debug.Log("All waves completed!");

        if (uiManager != null)
            uiManager.ShowGameOver(true);

        Time.timeScale = 0f;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (uiManager != null)
            uiManager.ShowPauseMenu();

        SetGameplayCursor(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (uiManager != null)
            uiManager.HidePauseMenu();

        SetGameplayCursor(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameActive = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (!Application.CanStreamedLevelBeLoaded(MainMenuScene))
        {
            Debug.LogError("Cannot return to the main menu: add Assets/Scenes/MainMenu.unity to Build Settings.", this);
            return;
        }

        SceneManager.LoadScene(MainMenuScene);
    }

    public static int GetSavedMoney(int fallback)
    {
        return PlayerPrefs.GetInt(MoneyPreferenceKey, fallback);
    }

    public static void SetSavedMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogError("Cannot save a negative amount of money.");
            return;
        }

        PlayerPrefs.SetInt(MoneyPreferenceKey, amount);
        PlayerPrefs.Save();

        GameManager gameManager = Instance;
        if (gameManager != null)
        {
            gameManager.playerMoney = amount;
            if (gameManager.uiManager != null)
                gameManager.uiManager.UpdateMoney(amount);
        }
    }

    public static int GetReserveAmmo()
    {
        return PlayerPrefs.GetInt(ReserveAmmoPreferenceKey, StartingReserveAmmo);
    }

    public static void AddReserveAmmo(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("Ammo pack amount must be greater than zero.");
            return;
        }

        SetReserveAmmo(GetReserveAmmo() + amount);
    }

    public static int TakeReserveAmmo(int requestedAmount)
    {
        if (requestedAmount <= 0)
            return 0;

        int reserveAmmo = GetReserveAmmo();
        int amountTaken = Mathf.Min(reserveAmmo, requestedAmount);
        if (amountTaken > 0)
            SetReserveAmmo(reserveAmmo - amountTaken);

        return amountTaken;
    }

    public static int GetHealthPackCount()
    {
        return PlayerPrefs.GetInt(HealthPackPreferenceKey, StartingHealthPacks);
    }

    public static void AddHealthPacks(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogError("Health pack amount must be greater than zero.");
            return;
        }

        SetHealthPackCount(GetHealthPackCount() + amount);
    }

    public static bool TryConsumeHealthPack()
    {
        int healthPacks = GetHealthPackCount();
        if (healthPacks <= 0)
            return false;

        SetHealthPackCount(healthPacks - 1);
        return true;
    }

    static void SetReserveAmmo(int amount)
    {
        PlayerPrefs.SetInt(ReserveAmmoPreferenceKey, Mathf.Max(0, amount));
        PlayerPrefs.Save();
    }

    static void SetHealthPackCount(int amount)
    {
        PlayerPrefs.SetInt(HealthPackPreferenceKey, Mathf.Max(0, amount));
        PlayerPrefs.Save();
    }

    public static bool TrySpendSavedMoney(int amount, int fallback = 500)
    {
        if (amount < 0)
            return false;

        GameManager gameManager = Instance;
        if (gameManager != null)
            return gameManager.SpendMoney(amount);

        int balance = GetSavedMoney(fallback);
        if (balance < amount)
            return false;

        SetSavedMoney(balance - amount);
        return true;
    }

    public static bool IsWeaponUnlocked(string weaponName)
    {
        return PlayerPrefs.GetInt(WeaponPreferencePrefix + weaponName, 0) == 1;
    }

    public static void UnlockWeapon(string weaponName)
    {
        PlayerPrefs.SetInt(WeaponPreferencePrefix + weaponName, 1);
        PlayerPrefs.Save();
    }
}
