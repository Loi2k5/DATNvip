using UnityEngine;

public class WeaponToggle : MonoBehaviour
{
    [SerializeField] private GameObject weapon;

    private bool isVisible = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isVisible = !isVisible;
            weapon.SetActive(isVisible);
        }
    }
}
