using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    public GameObject gun;
    public GameObject scar;
    public GameObject shotgun;

    public void ChooseGun()
    {
        DisableAllWeapons();
        gun.SetActive(true);
        ClosePanel();
    }

    public void ChooseScar()
    {
        DisableAllWeapons();
        scar.SetActive(true);
        ClosePanel();
    }

    public void ChooseShotgun()
    {
        DisableAllWeapons();
        shotgun.SetActive(true);
        ClosePanel();
    }

    void DisableAllWeapons()
    {
        gun.SetActive(false);
        scar.SetActive(false);
        shotgun.SetActive(false);
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}

