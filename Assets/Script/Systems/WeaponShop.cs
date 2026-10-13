using UnityEngine;

public class WeaponShop : MonoBehaviour
{
    [System.Serializable]
    public class WeaponItem
    {
        public string weaponName;
        public GameObject weaponPrefab;
        public int price;
        public bool isPurchased = false;
        public int damage = 25;
        public float fireRate = 0.2f;
        public int maxAmmo = 30;
        public float reloadTime = 1.5f;
        public float range = 100f;
    }

    [Header("Available Weapons")]
    public WeaponItem[] weaponsForSale;

    [Header("Player Reference")]
    public Transform weaponHolder;

    void Start()
    {
        if (weaponsForSale == null)
            weaponsForSale = new WeaponItem[0];

        if (weaponHolder == null)
        {
            WeaponSwitcher switcher = FindAnyObjectByType<WeaponSwitcher>();
            if (switcher != null)
                weaponHolder = switcher.transform;
        }

        foreach (WeaponItem weapon in weaponsForSale)
        {
            if (weapon == null)
                continue;

            if (weapon.isPurchased)
                GameManager.UnlockWeapon(weapon.weaponName);

            if (GameManager.IsWeaponUnlocked(weapon.weaponName))
            {
                weapon.isPurchased = true;
                if (weaponHolder != null)
                    AddWeaponToLoadout(weapon);
            }
        }

        ShopUI shopUI = FindAnyObjectByType<ShopUI>();
        if (shopUI != null)
            shopUI.RefreshButtonStates();
    }

    public bool BuyWeapon(int weaponIndex)
    {
        if (weaponsForSale == null)
            return false;

        if (weaponIndex < 0 || weaponIndex >= weaponsForSale.Length)
            return false;

        WeaponItem weapon = weaponsForSale[weaponIndex];
        if (weapon == null || weapon.weaponPrefab == null || weapon.price < 0)
        {
            Debug.LogError("Cannot buy weapon: assign a prefab and a non-negative price.");
            return false;
        }
        if (weaponHolder != null && weaponHolder.GetComponent<WeaponSwitcher>() == null)
        {
            Debug.LogError("Cannot buy weapon: the weapon holder needs a WeaponSwitcher.");
            return false;
        }

        // Kiểm tra đã mua chưa
        if (weapon.isPurchased)
        {
            Debug.Log("You already own this weapon!");
            return false;
        }

        // Kiểm tra đủ tiền không
        if (GameManager.TrySpendSavedMoney(weapon.price))
        {
            weapon.isPurchased = true;
            GameManager.UnlockWeapon(weapon.weaponName);
            if (weaponHolder != null)
                AddWeaponToLoadout(weapon);

            Debug.Log("Purchased: " + weapon.weaponName);
            return true;
        }

        Debug.Log("Not enough money!");
        return false;
    }

    void AddWeaponToLoadout(WeaponItem weapon)
    {
        if (weaponHolder == null || weapon.weaponPrefab == null)
        {
            Debug.LogError("Cannot add purchased weapon: assign its prefab and the player's weapon holder.");
            return;
        }

        WeaponSwitcher switcher = weaponHolder.GetComponent<WeaponSwitcher>();
        if (switcher == null)
        {
            Debug.LogError("Cannot add purchased weapon: WeaponSwitcher is missing from the weapon holder.");
            return;
        }

        GameObject newWeapon = Instantiate(weapon.weaponPrefab, weaponHolder);
        newWeapon.SetActive(false);
        Gun gun = newWeapon.GetComponent<Gun>();
        if (gun == null)
            gun = newWeapon.AddComponent<Gun>();
        gun.gunName = weapon.weaponName;
        gun.damage = weapon.damage;
        gun.fireRate = weapon.fireRate;
        gun.maxAmmo = weapon.maxAmmo;
        gun.reloadTime = weapon.reloadTime;
        gun.range = weapon.range;

        Gun[] currentWeapons = switcher.weapons ?? new Gun[0];
        Gun[] newWeapons = new Gun[currentWeapons.Length + 1];
        currentWeapons.CopyTo(newWeapons, 0);
        newWeapons[newWeapons.Length - 1] = gun;
        switcher.weapons = newWeapons;
    }

    public bool BuyAmmo(int amount, int price)
    {
        if (amount <= 0 || price < 0)
        {
            Debug.LogError("Ammo pack amount must be positive and its price cannot be negative.");
            return false;
        }

        if (GameManager.TrySpendSavedMoney(price))
        {
            GameManager.AddReserveAmmo(amount);
            Debug.Log("Purchased ammo: +" + amount + " rounds. Reserve: " + GameManager.GetReserveAmmo());
            return true;
        }

        Debug.Log("Not enough money for ammo!");
        return false;
    }

    public bool BuyHealthPack(int price)
    {
        if (price < 0)
        {
            Debug.LogError("Cannot buy a health pack with a negative price.");
            return false;
        }

        if (GameManager.TrySpendSavedMoney(price))
        {
            GameManager.AddHealthPacks(1);
            Debug.Log("Purchased health pack. Inventory: " + GameManager.GetHealthPackCount());
            return true;
        }

        Debug.Log("Not enough money for health pack!");
        return false;
    }
}
