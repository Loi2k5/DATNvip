using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject dialogueCanvas;     // Gán Dialogue Canvas
    public TypingEffect typingEffect;     // Gắn script TypingEffect từ dialogueCanvas
    public GameObject storyText;          // Gán StoryText (bảng story bạn đã làm)

    private bool playerEntered = false;
    private bool dialogueStarted = false;
    private bool storyShown = false;
    private bool storyDismissed = false;

    void Start()
    {
        if (dialogueCanvas != null)
            dialogueCanvas.SetActive(false);

        if (storyText != null)
            storyText.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !dialogueStarted)
        {
            playerEntered = true;
            dialogueStarted = true;

            if (dialogueCanvas != null)
                dialogueCanvas.SetActive(true);

            if (typingEffect != null)
                typingEffect.StartTyping(); // Sử dụng fullText có sẵn
        }
    }

    void Update()
    {
        if (!dialogueStarted) return;

        // Khi TypingEffect chạy xong thì hiện storyText
        if (!typingEffect.IsTyping() && !storyShown)
        {
            storyText.SetActive(true);
            storyShown = true;
        }

        // Nhấn Space để ẩn storyText
        if (storyShown && !storyDismissed && Input.GetKeyDown(KeyCode.Space))
        {
            storyText.SetActive(false);
            storyDismissed = true;
        }
    }
}
