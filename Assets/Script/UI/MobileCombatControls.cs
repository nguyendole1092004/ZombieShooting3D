using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MobileCombatControls : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] WeaponSwitcher weaponSwitcher;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Combat Buttons")]
    [SerializeField] Button jumpButton;
    [SerializeField] Button reloadButton;
    [SerializeField] Button switchWeaponButton;
    [SerializeField] Button useHealthPackButton;

    [Header("Menu Buttons")]
    [SerializeField] Button openShopButton;
    [SerializeField] Button pauseButton;

    bool fireHeld;
    TMP_Text healthPackButtonLabel;

    void Awake()
    {
        if (weaponSwitcher == null)
            Debug.LogError("Assign the player's WeaponSwitcher to MobileCombatControls.", this);
        if (playerHealth == null)
            Debug.LogError("Assign the player's PlayerHealth to MobileCombatControls.", this);
        if (playerMovement == null)
            Debug.LogError("Assign the player's PlayerMovement to MobileCombatControls.", this);
        if (jumpButton == null)
            Debug.LogError("Create a mobile Jump Button and assign it to MobileCombatControls.", this);
        if (useHealthPackButton != null)
            healthPackButtonLabel = useHealthPackButton.GetComponentInChildren<TMP_Text>();
    }

    void OnEnable()
    {
        AddListener(jumpButton, RequestJump);
        AddListener(reloadButton, ReloadCurrentWeapon);
        AddListener(switchWeaponButton, SwitchToNextWeapon);
        AddListener(useHealthPackButton, UseHealthPack);
        AddListener(openShopButton, OpenShop);
        AddListener(pauseButton, PauseGame);
    }

    void OnDisable()
    {
        RemoveListener(jumpButton, RequestJump);
        RemoveListener(reloadButton, ReloadCurrentWeapon);
        RemoveListener(switchWeaponButton, SwitchToNextWeapon);
        RemoveListener(useHealthPackButton, UseHealthPack);
        RemoveListener(openShopButton, OpenShop);
        RemoveListener(pauseButton, PauseGame);

        Gun currentGun = weaponSwitcher != null ? weaponSwitcher.GetCurrentWeapon() : null;
        if (currentGun != null)
            currentGun.SetMobileFireHeld(false);
    }

    void Update()
    {
        Gun currentGun = weaponSwitcher != null ? weaponSwitcher.GetCurrentWeapon() : null;
        if (currentGun != null)
            currentGun.SetMobileFireHeld(fireHeld);

        if (useHealthPackButton != null && playerHealth != null)
        {
            useHealthPackButton.interactable =
                GameManager.GetHealthPackCount() > 0 &&
                playerHealth.currentHealth < playerHealth.maxHealth &&
                !playerHealth.IsDead();

            if (healthPackButtonLabel != null)
            {
                string inventoryLabel = "HEAL x" + GameManager.GetHealthPackCount();
                if (healthPackButtonLabel.text != inventoryLabel)
                    healthPackButtonLabel.text = inventoryLabel;
            }
        }
    }

    public void SetFireHeld(bool isHeld)
    {
        fireHeld = isHeld;
    }

    public void ReloadCurrentWeapon()
    {
        Gun currentGun = weaponSwitcher != null ? weaponSwitcher.GetCurrentWeapon() : null;
        if (currentGun != null)
            currentGun.TryReload();
    }

    public void RequestJump()
    {
        if (playerMovement != null)
            playerMovement.RequestJump();
    }

    public void SwitchToNextWeapon()
    {
        if (weaponSwitcher != null)
            weaponSwitcher.NextWeapon();
    }

    public void UseHealthPack()
    {
        if (playerHealth != null)
            playerHealth.UseHealthPack();
    }

    public void OpenShop()
    {
        UIManager uiManager = FindAnyObjectByType<UIManager>();
        if (uiManager != null)
            uiManager.ToggleShop();
        else
            Debug.LogError("Cannot open the shop: no UIManager exists in the gameplay scene.", this);
    }

    public void PauseGame()
    {
        GameManager gameManager = GameManager.Instance;
        if (gameManager != null)
            gameManager.PauseGame();
        else
            Debug.LogError("Cannot pause the game: no GameManager exists in the gameplay scene.", this);
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
}
