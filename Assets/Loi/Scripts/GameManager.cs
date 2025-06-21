using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int currentEnergy;
    [SerializeField] private int energyThreshold = 3;

    [Header("UI")]
    [SerializeField] private Image energyBar;
    [SerializeField] private GameObject skillUpgradeUI;
    [SerializeField] private TextMeshProUGUI pointText;

    private int currentPoints = 0;

    private bool skillUIShown = false;

    void Start()
    {
        UpdateUI();
        currentEnergy = 0;
        skillUIShown = false;
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

    private void UpdateUI()
    {
        if (pointText != null)
        {
            pointText.text = $" {currentPoints}";
        }
    }


}
