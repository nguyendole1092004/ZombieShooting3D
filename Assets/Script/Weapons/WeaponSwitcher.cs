using UnityEngine;

public class WeaponSwitcher : MonoBehaviour
{
    public Gun[] weapons; // Array chứa tất cả vũ khí
    private int currentWeaponIndex = 0;

    void Start()
    {
        if (weapons == null)
            weapons = new Gun[0];

        SelectWeapon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Length >= 1)
            SwitchWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Length >= 2)
            SwitchWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3) && weapons.Length >= 3)
            SwitchWeapon(2);

        if (weapons.Length < 2)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
            NextWeapon();
        else if (scroll < 0f)
            PreviousWeapon();
    }

    public void SwitchWeapon(int index)
    {
        if (weapons == null || index < 0 || index >= weapons.Length || weapons[index] == null)
            return;

        if (index != currentWeaponIndex || !weapons[index].gameObject.activeSelf)
        {
            currentWeaponIndex = index;
            SelectWeapon();
        }
    }

    public void NextWeapon()
    {
        SwitchRelative(1);
    }

    public void PreviousWeapon()
    {
        SwitchRelative(-1);
    }

    void SwitchRelative(int direction)
    {
        if (weapons == null || weapons.Length < 2)
            return;

        for (int offset = 1; offset <= weapons.Length; offset++)
        {
            int index = (currentWeaponIndex + direction * offset + weapons.Length) % weapons.Length;
            if (weapons[index] != null)
            {
                SwitchWeapon(index);
                return;
            }
        }
    }

    void SelectWeapon()
    {
        if (weapons == null)
            return;

        for (int i = 0; i < weapons.Length; i++)
        {
            Gun weapon = weapons[i];
            if (weapon != null)
                weapon.gameObject.SetActive(i == currentWeaponIndex);
        }
    }

    public Gun GetCurrentWeapon()
    {
        if (weapons != null && currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Length)
            return weapons[currentWeaponIndex];
        return null;
    }
}
