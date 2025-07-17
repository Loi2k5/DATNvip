using UnityEngine;

public class PetSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject kikiPrefab;
    public GameObject samuraiPrefab;
    public GameObject droneBitPrefab;

    void Start()
    {
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
        }

        if (petToSpawn != null)
        {
            Instantiate(petToSpawn, spawnPoint.position, Quaternion.identity);
        }*/
    }
}
