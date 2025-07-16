using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private GameManager gameManager; 
    [SerializeField]  private AudioManager audioManager;
    

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

            if (gameManager != null)
                gameManager.AddPoint(1);
            audioManager.PlayEnergySound();
            Destroy(collision.gameObject);
        }
    }
}
