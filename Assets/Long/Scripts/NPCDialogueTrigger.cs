using UnityEngine;

public class NPCDialogueTrigger : MonoBehaviour
{
    public string message = "Bố Mẹ tự hào về con!";
    private bool playerInRange = false;
    private DialogueManager dialogueManager;

    private void Start()
    {
        // Sửa ở đây: dùng FindFirstObjectByType thay vì FindObjectOfType (bị deprecated)
        dialogueManager = FindFirstObjectByType<DialogueManager>();

        if (dialogueManager == null)
        {
            Debug.LogError("Không tìm thấy DialogueManager trong scene!");
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogueManager.dialoguePanel.activeSelf)
            {
                dialogueManager.ShowDialogue(message);
            }
            else if (dialogueManager.IsTyping())
            {
                dialogueManager.SkipToFullText(); // nhấn E lần nữa để hiện toàn bộ
            }
            else
            {
                dialogueManager.HideDialogue(); // nếu hiện xong thì ẩn đi
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogueManager.HideDialogue(); // tự động ẩn khi rời xa
        }
    }
}
