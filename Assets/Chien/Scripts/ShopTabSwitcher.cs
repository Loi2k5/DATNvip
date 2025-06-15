using UnityEngine;

public class ShopTabSwitcher : MonoBehaviour
{
    public GameObject weaponUI;
    public GameObject petUI;

    public void ShowWeaponTab()
    {
        weaponUI.SetActive(true);
        petUI.SetActive(false);
    }

    public void ShowPetTab()
    {
        weaponUI.SetActive(false);
        petUI.SetActive(true);
    }
}
