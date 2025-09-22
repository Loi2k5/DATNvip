using UnityEngine;

public class NPCQuestGiver : MonoBehaviour
{
    private bool alreadyTalked = false; // để NPC không lặp lại nhiều lần

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (!QuestManager.Instance.questAccepted)
        {
            QuestManager.Instance.AcceptQuest();
            Debug.Log("Đã nhận nhiệm vụ: Nhặt 20 năng lượng!");
        }
        else if (QuestManager.Instance.questCompleted && !QuestManager.Instance.questTurnedIn && !alreadyTalked)
        {
            QuestManager.Instance.TurnInQuest();
            alreadyTalked = true;
            Debug.Log("Nhiệm vụ hoàn thành! Cổng đã mở.");
        }
    }
}
