using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameManager gameManager; 
    [SerializeField]  private AudioManager audioManager;
    public QuestManager questManager;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Player player = GetComponent<Player>();
            player.TakeDamage(10f);
        }
        else if (collision.CompareTag("Usb"))
        {
            Debug.Log("Win");
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Energy"))
        {
            /*if (gameManager != null)
                gameManager.AddEnergy();*/
            if (QuestManager.Instance != null && QuestManager.Instance.questAccepted)
            {
                QuestManager.Instance.AddEnergy(1);
            }
            if (gameManager != null)
                PointManager.Instance.AddPoint(1);
            audioManager.PlayEnergySound();
            Destroy(collision.gameObject);
        }
    }
}
