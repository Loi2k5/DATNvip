using UnityEngine;

public class PetSpawner : MonoBehaviour
{
    public GameObject[] petPrefabs;

    void Start()
    {
        string selectedPet = PlayerPrefs.GetString("SelectedPet", "");

        if (!string.IsNullOrEmpty(selectedPet))
        {
            GameObject petToSpawn = null;

            foreach (GameObject pet in petPrefabs)
            {
                if (pet.name == selectedPet)
                {
                    petToSpawn = pet;
                    break;
                }
            }

            if (petToSpawn != null)
            {
                Instantiate(petToSpawn, transform.position, Quaternion.identity);
                Debug.Log("Đã spawn pet: " + selectedPet);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy pet: " + selectedPet);
            }
        }
    }
}
