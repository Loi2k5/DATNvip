using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    public void ReplayGame()
    {
        // Load lại scene hiện tại (chơi lại)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("EndScene"); // thay bằng tên scene menu của bạn
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Thoát game"); // Chỉ hoạt động khi build
    }
}
