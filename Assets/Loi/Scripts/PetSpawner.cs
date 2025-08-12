using UnityEngine;

public class PetSpawner : MonoBehaviour
{
    public GameObject[] petPrefabs;

    void Start()
    {
        if (PetPurchaseManager.Instance == null || PetPurchaseManager.Instance.purchasedPets.Count == 0)
        {
            Debug.Log("Không có pet nào được mua → không spawn.");
            return;
        }

        int spawnIndex = 0;
        foreach (string petID in PetPurchaseManager.Instance.purchasedPets)
        {
            GameObject petToSpawn = System.Array.Find(petPrefabs, p => p.name == petID);
            if (petToSpawn != null)
            {
                Vector3 pos = transform.position + Vector3.right * spawnIndex * 1.5f;
                Instantiate(petToSpawn, pos, Quaternion.identity);
                Debug.Log("Spawn pet: " + petID);
                spawnIndex++;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy prefab pet: " + petID);
            }
        }
    }
}
