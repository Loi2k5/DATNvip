using UnityEngine;
/// <summary>
/// dùng trong scene chơi
/// </summary>
public class PetSummoner : MonoBehaviour
{
    [System.Serializable]
    public class PetData
    {
        public string petID;
        public GameObject petPrefab;
    }

    [SerializeField] private PetData[] pets;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("[PetSummoner] Không tìm thấy đối tượng Player.");
            return;
        }

        foreach (PetData pet in pets)
        {
            if (PlayerPrefs.GetInt(pet.petID, 0) == 1)
            {
                Instantiate(pet.petPrefab, player.transform.position, Quaternion.identity);
            }
        }
    }
}