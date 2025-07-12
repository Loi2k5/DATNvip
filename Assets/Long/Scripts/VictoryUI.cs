using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryUI : MonoBehaviour
{
    public void OnMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Tên scene menu chính của bạn
    }

    public void OnReplay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Tải lại màn chơi hiện tại
    }

    public void OnQuit()
    {
        Application.Quit(); // Thoát game (chỉ hoạt động khi build game)
        Debug.Log("Quit game"); // Dùng cho khi chạy trong Editor
    }
}
