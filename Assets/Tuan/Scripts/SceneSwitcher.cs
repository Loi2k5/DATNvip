using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    // Tên scene bạn muốn chuyển đến
    [SerializeField] private string targetSceneName;

    // Hàm này gọi khi nhấn nút
    public void SwitchScene()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Scene đích chưa được đặt tên!");
        }
    }
}