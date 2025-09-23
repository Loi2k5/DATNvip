using UnityEngine;

public class PetSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject kikiPrefab;
    public GameObject samuraiPrefab;
    public GameObject droneBitPrefab;

    void Start()
    {
<<<<<<< HEAD
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
=======
        /*string selected = GameManager.Instance.SelectedPet;
        GameObject petToSpawn = null;

        switch (selected)
        {
            case "KiKi":
                petToSpawn = kikiPrefab;
                break;
            case "Automic Samurai":
                petToSpawn = samuraiPrefab;
                break;
            case "Drone Bit":
                petToSpawn = droneBitPrefab;
                break;
            default:
                Debug.Log("Chưa chọn pet!");
                break;
>>>>>>> long
        }

        if (petToSpawn != null)
        {
            Instantiate(petToSpawn, spawnPoint.position, Quaternion.identity);
        }*/
    }
}
