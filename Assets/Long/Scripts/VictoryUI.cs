using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    [Header("Optional Effects")]
    public AudioSource clickSound;
    public Animator transitionAnimator; // Gắn Animator có hiệu ứng fade (nếu có)
    public Button mainMenuButton;
    public Button replayButton;
    public Button quitButton;

    private bool isTransitioning = false;

    public void OnMainMenu()
    {
        if (isTransitioning) return;
        StartCoroutine(LoadSceneWithEffect("MainMenu"));
    }

    public void OnReplay()
    {
        if (isTransitioning) return;
        StartCoroutine(LoadSceneWithEffect(SceneManager.GetActiveScene().name));
    }

    public void OnQuit()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        if (clickSound) clickSound.Play();
        DisableAllButtons();

        // Gắn thêm hiệu ứng thoát nếu muốn
        Debug.Log("Quit game");
        Application.Quit();
    }

    private System.Collections.IEnumerator LoadSceneWithEffect(string sceneName)
    {
        isTransitioning = true;
        if (clickSound) clickSound.Play();
        DisableAllButtons();

        if (transitionAnimator)
        {
            transitionAnimator.SetTrigger("Start");
            yield return new WaitForSeconds(1.0f); // thời gian chờ animation
        }

        SceneManager.LoadScene(sceneName);
    }

    private void DisableAllButtons()
    {
        if (mainMenuButton) mainMenuButton.interactable = false;
        if (replayButton) replayButton.interactable = false;
        if (quitButton) quitButton.interactable = false;
    }
}
