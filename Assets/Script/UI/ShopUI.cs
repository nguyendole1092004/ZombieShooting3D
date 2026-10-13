using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public class ShopUI : MonoBehaviour
{
    [Header("Shop Reference")]
    public WeaponShop weaponShop;
    [SerializeField] TMP_Text creditsText;

    [Header("UI Buttons")]
    public Button[] weaponButtons;
    public Button ammoButton;
    public Button healthButton;
    public Button closeButton;

    [Header("Prices")]
    [FormerlySerializedAs("ammoPack")]
    public int ammoPackPrice = 50;
    public int ammoPackAmount = 90;
    [FormerlySerializedAs("healthPack")]
    public int healthPackPrice = 100;

    MainMenuController mainMenuController;

    void Start()
    {
        if (weaponButtons == null)
            weaponButtons = new Button[0];

        for (int i = 0; i < weaponButtons.Length; i++)
        {
            if (weaponButtons[i] == null)
                continue;

            int index = i;
            weaponButtons[i].onClick.AddListener(() => OnWeaponButtonClicked(index));
        }

        if (ammoButton != null)
            ammoButton.onClick.AddListener(OnAmmoButtonClicked);

        if (healthButton != null)
            healthButton.onClick.AddListener(OnHealthButtonClicked);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseShop);

        RefreshButtonStates();
    }

    public void ConfigureForMainMenu(MainMenuController controller)
    {
        mainMenuController = controller;
    }

    void OnWeaponButtonClicked(int weaponIndex)
    {
        if (weaponShop != null)
            weaponShop.BuyWeapon(weaponIndex);

        RefreshButtonStates();
    }

    void OnAmmoButtonClicked()
    {
        if (weaponShop != null)
            weaponShop.BuyAmmo(ammoPackAmount, ammoPackPrice);

        RefreshButtonStates();
    }

    void OnHealthButtonClicked()
    {
        if (weaponShop != null)
            weaponShop.BuyHealthPack(healthPackPrice);

        RefreshButtonStates();
    }

    public void RefreshButtonStates()
    {
        if (weaponButtons == null)
            weaponButtons = new Button[0];

        if (creditsText != null)
            creditsText.text = "SHOP  |  CREDITS $" + GameManager.GetSavedMoney(500);
        if (mainMenuController != null)
            mainMenuController.RefreshBalanceDisplay();

        SetButtonLabel(
            ammoButton,
            "AMMO +" + ammoPackAmount + "  $" + ammoPackPrice + "\nINVENTORY: " + GameManager.GetReserveAmmo());
        SetButtonLabel(
            healthButton,
            "HEALTH PACK +" + GameManager.HealthPackHealAmount + " HP  $" + healthPackPrice +
            "\nINVENTORY: " + GameManager.GetHealthPackCount());

        if (weaponShop == null || weaponShop.weaponsForSale == null)
            return;

        for (int i = 0; i < weaponButtons.Length && i < weaponShop.weaponsForSale.Length; i++)
        {
            Button button = weaponButtons[i];
            WeaponShop.WeaponItem weapon = weaponShop.weaponsForSale[i];
            if (button == null || weapon == null)
                continue;

            weapon.isPurchased = weapon.isPurchased || GameManager.IsWeaponUnlocked(weapon.weaponName);
            button.interactable = !weapon.isPurchased;

            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = weapon.isPurchased
                    ? weapon.weaponName + "\n(OWNED)"
                    : weapon.weaponName + "\n$" + weapon.price;
            }
        }
    }

    void SetButtonLabel(Button button, string label)
    {
        if (button == null)
            return;

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
            buttonText.text = label;
    }

    void CloseShop()
    {
        if (mainMenuController != null)
        {
            mainMenuController.ShowMainMenu();
            return;
        }

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
            uiManager.ToggleShop();
    }
}
