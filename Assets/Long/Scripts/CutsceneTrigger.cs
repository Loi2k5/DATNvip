using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public string message = "Sao giờ này mới đến ";
    private bool playerInRange = false;
    private bool hasTriggered = false;

    private DialogueManager dialogueManager;

    void Start()
    {
        // Sử dụng cách mới để tránh cảnh báo CS0618
        dialogueManager = Object.FindFirstObjectByType<DialogueManager>();
    }

    void Update()
    {
        if (playerInRange && !hasTriggered && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.ShowDialogue(message);
            hasTriggered = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
