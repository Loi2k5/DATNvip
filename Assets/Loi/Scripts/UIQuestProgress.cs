using UnityEngine;
using TMPro;

public class UIQuestProgress : MonoBehaviour
{
    public static UIQuestProgress Instance;
    public TextMeshProUGUI questText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateProgress(int current, int required)
    {
        questText.text = $"Nhiệm vụ: {current}/{required}";
    }

    public void ShowProgress(bool show)
    {
        questText.gameObject.SetActive(show);
        if (show) UpdateProgress(0, QuestManager.Instance.requiredEnergy);
    }
}
