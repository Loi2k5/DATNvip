using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public int requiredEnergy = 20;
    public int currentEnergy = 0;

    public bool questAccepted = false;
    public bool questCompleted = false;
    public bool questTurnedIn = false;

    [Header("Liên kết trong Inspector")]
    public GameObject enemySpawner; // gắn object spawn quái ở đây
    public GameObject portalToNextScene; // gắn cổng qua màn

    private void Awake()
    {
        Instance = this;
    }

    public void AddEnergy(int amount)
    {
        if (!questAccepted || questCompleted) return;

        currentEnergy += amount;
        Debug.Log("Nhặt năng lượng, hiện có: " + currentEnergy);

        if (currentEnergy >= requiredEnergy)
        {
            currentEnergy = requiredEnergy;
            questCompleted = true;
            enemySpawner.SetActive(false); // tắt spawn quái khi đủ
            HideAllEnemies();              // ẩn hết quái còn sống
        }

        UIQuestProgress.Instance.UpdateProgress(currentEnergy, requiredEnergy);
    }

    public void AcceptQuest()
    {
        questAccepted = true;
        currentEnergy = 0;
        questCompleted = false;
        UIQuestProgress.Instance.ShowProgress(true);

        enemySpawner.SetActive(true); // bật spawn quái khi nhận
    }

    public void TurnInQuest()
    {
        if (questCompleted && !questTurnedIn)
        {
            questTurnedIn = true;
            UIQuestProgress.Instance.ShowProgress(false);
            portalToNextScene.SetActive(true); // mở cổng khi trả nhiệm vụ
        }
    }

    void HideAllEnemies()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }
}
