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

        // Nếu chưa nhận quest → hiện thoại nhận
        if (!QuestManager.Instance.questAccepted)
            dialogueText.text = acceptDialogue[dialogueIndex];
        // Nếu hoàn thành quest → hiện thoại trả
        else if (QuestManager.Instance.questCompleted && !QuestManager.Instance.questTurnedIn)
            dialogueText.text = turnInDialogue[dialogueIndex];
        else
        {
            dialogueText.text = "Quay lại khi hoàn thành nhiệm vụ!";
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (!QuestManager.Instance.questAccepted)
        {
            if (dialogueIndex < acceptDialogue.Length)
                dialogueText.text = acceptDialogue[dialogueIndex];
            else
            {
                EndDialogue();
                QuestManager.Instance.AcceptQuest(); // nhận nhiệm vụ
            }
        }
        else if (QuestManager.Instance.questCompleted && !QuestManager.Instance.questTurnedIn)
        {
            if (dialogueIndex < turnInDialogue.Length)
                dialogueText.text = turnInDialogue[dialogueIndex];
            else
            {
                EndDialogue();
                QuestManager.Instance.TurnInQuest(); // trả nhiệm vụ
            }
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);

        // ✅ Mở lại di chuyển player
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
