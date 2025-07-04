using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Gọi khi nhấn nút Play
    public void OnPlayButton()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1); // Chuyển sang màn tiếp theo
    }

    // Gọi khi nhấn nút Quit
    public void OnQuitButton()
    {
        Debug.Log("Quitting game...");
        Application.Quit(); // Thoát game khi build
    }
    public void OnBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Đảm bảo tên scene là chính xác
    }

}
