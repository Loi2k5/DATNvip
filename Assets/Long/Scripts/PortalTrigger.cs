using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalTrigger : MonoBehaviour
{
    public string sceneToLoad = "End"; // 👈 Đổi thành tên scene kết của bạn

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && GameFlags.bossIsDead)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
