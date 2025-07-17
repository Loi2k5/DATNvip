using UnityEngine;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    public GameObject dialoguePanel;                   // Panel chứa đoạn hội thoại
    public TextMeshProUGUI dialogueText;               // Text TMP để hiển thị lời thoại
    public string message = "Shin... Cuối cùng chúng ta cũng gặp lại con. Bố mẹ tự hào về con hơn bất cứ điều gì!";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(true);
            dialogueText.text = message;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialoguePanel.SetActive(false);
        }
    }
}
