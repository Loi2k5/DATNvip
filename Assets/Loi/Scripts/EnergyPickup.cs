using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Energy"))
        {
            QuestManager.Instance.AddEnergy(1);
            Destroy(gameObject);
        }
    }
}
