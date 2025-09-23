using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueUI;        // UI hội thoại
    public TextMeshProUGUI dialogueText; // text hiển thị
    public string[] acceptDialogue;      // thoại khi nhận nhiệm vụ
    public string[] turnInDialogue;      // thoại khi trả nhiệm vụ
    public KeyCode interactKey = KeyCode.E;

    private bool isPlayerInRange = false;
    private int dialogueIndex = 0;
    private bool isTalking = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(interactKey))
        {
            if (!isTalking) StartDialogue();
            else NextDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogueIndex = 0;
        dialogueUI.SetActive(true);

        Player player = FindObjectOfType<Player>();
        if (player != null) player.canMove = false;

        // ✅ Hiển thị đúng đoạn thoại đầu tiên theo trạng thái nhiệm vụ
        if (!QuestManager.Instance.questAccepted)
        {
            dialogueText.text = acceptDialogue.Length > 0 ? acceptDialogue[0] : "Xin chào, hãy nhận nhiệm vụ!";
        }
        else if (QuestManager.Instance.questCompleted && !QuestManager.Instance.questTurnedIn)
        {
            dialogueText.text = turnInDialogue.Length > 0 ? turnInDialogue[0] : "Cảm ơn, nhiệm vụ hoàn thành!";
        }
        else if (QuestManager.Instance.questAccepted && !QuestManager.Instance.questCompleted)
        {
            dialogueText.text = "Hãy tiêu diệt và thu thập 20 năng lượng!";
        }
        else if (QuestManager.Instance.questTurnedIn)
        {
            dialogueText.text = "Cảm ơn, bạn đã giúp tôi xong việc rồi!";
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (!QuestManager.Instance.questAccepted)
        {
            if (dialogueIndex < acceptDialogue.Length)
            {
                dialogueText.text = acceptDialogue[dialogueIndex];
            }
            else
            {
                QuestManager.Instance.AcceptQuest(); // Nhận nhiệm vụ
                EndDialogue();
            }
        }
        else if (QuestManager.Instance.questCompleted && !QuestManager.Instance.questTurnedIn)
        {
            if (dialogueIndex < turnInDialogue.Length)
            {
                dialogueText.text = turnInDialogue[dialogueIndex];
            }
            else
            {
                QuestManager.Instance.TurnInQuest(); // Trả nhiệm vụ
                EndDialogue();
            }
        }
        else if (QuestManager.Instance.questAccepted && !QuestManager.Instance.questCompleted)
        {
            dialogueText.text = "Quay lại khi hoàn thành nhiệm vụ!";
            EndDialogue();
        }
        else if (QuestManager.Instance.questTurnedIn)
        {
            dialogueText.text = "Cảm ơn, bạn đã giúp tôi xong việc rồi!";
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);

        // ✅ Cho player di chuyển lại
        Player player = FindObjectOfType<Player>();
        if (player != null) player.canMove = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            isPlayerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            EndDialogue();
        }
    }
}
