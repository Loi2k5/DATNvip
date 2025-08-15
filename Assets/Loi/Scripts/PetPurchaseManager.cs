using System.Collections.Generic;
using UnityEngine;

public class PetPurchaseManager : MonoBehaviour
{
    public static PetPurchaseManager Instance;

    public List<string> purchasedPets = new List<string>();

    void Awake()
    {
        // Singleton tạm cho phiên chơi này
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại qua scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPet(string petID)
    {
        if (!purchasedPets.Contains(petID))
        {
            purchasedPets.Add(petID);
            Debug.Log("Đã thêm pet vào danh sách spawn: " + petID);
        }
    }

    public void ResetPurchasedPets()
    {
        purchasedPets.Clear();
    }
}
