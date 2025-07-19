using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int currentEnergy;
    [SerializeField] private int energyThreshold = 3;

    [Header("UI")]
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject skillUpgradeUI;
    [SerializeField] private TextMeshProUGUI pointText;

    private int currentPoints = 0;//diem nang luong mua pet

    private bool skillUIShown = false;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;

    void Start()
    {
        UpdateUI();
        currentEnergy = 0;
        skillUIShown = false;  
        gameOverMenu.SetActive(false); pauseMenu.SetActive(false);
        UpdateEnergyBar();

        if (skillUpgradeUI != null)
            skillUpgradeUI.SetActive(false);

        Time.timeScale = 1f; // đảm bảo game chạy bình thường khi bắt đầu
    }

    // Gọi hàm này từ script Enemy khi bị tiêu diệt
    public void AddEnergy()
    {
        if (skillUIShown || currentEnergy >= energyThreshold)
            return;

        currentEnergy++;
        UpdateEnergyBar();

        if (currentEnergy >= energyThreshold)
        {
            ShowSkillUpgradeUI();
        }
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            float fillAmount = Mathf.Clamp01((float)currentEnergy / energyThreshold);
            energyBar.fillAmount = fillAmount;
        }
    }

    private void ShowSkillUpgradeUI()
    {
        skillUIShown = true;

        if (skillUpgradeUI != null)
        {
            skillUpgradeUI.SetActive(true);
        }

        Time.timeScale = 0f; // TẠM DỪNG GAME tại đây
    }

    // Gọi từ nút "Xác nhận nâng cấp" để reset năng lượng và tiếp tục game
    public void ResetEnergy()
    {
        currentEnergy = 0;
        skillUIShown = false;
        UpdateEnergyBar();

        if (skillUpgradeUI != null)
            skillUpgradeUI.SetActive(false);

        Time.timeScale = 1f; // CHẠY LẠI GAME
    }
    public void AddPoint(int amount)
    {
        currentPoints += amount;
        UpdateUI();
    }

    public int GetCurrentPoints()
    {
        return currentPoints;
    }

    private void UpdateUI()
    {
        if (pointText != null)
        {
            pointText.text = $" {currentPoints}";
        }
    }
    public void GameOverMenu()
    {
        Debug.Log("GameOverMenu được gọi");

        if (skillUpgradeUI != null) skillUpgradeUI.SetActive(false);
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(true);

        // Không cần Time.timeScale ở đây nếu đã gọi trong Die()
    }



    public void PauseGameMenu()
    {
        pauseMenu.SetActive(true);
        gameOverMenu.SetActive(false);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f; // Chạy lại thời gian
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }    
    public void ContinuesGame()
    {
        ResumeGame();
    }    
}
