using UnityEngine;
using TMPro;

public class NPCDialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;   // Panel UI thoại
    [SerializeField] private TextMeshProUGUI dialogueText;

    [TextArea]
    [SerializeField]
    private string[] dialogues = new string[]
    {
        "Sau khi vượt qua lũ quái vật đầy nguy hiểm...",
    "Nhân vật chính cuối cùng cũng tìm thấy bố mẹ...",
    "Ánh mắt lo lắng của họ dần tan biến khi nhìn thấy con vẫn an toàn...",
    "Cả gia đình ôm chầm lấy nhau trong niềm hạnh phúc vỡ òa...",
    "Những giọt nước mắt lăn dài, không còn vì sợ hãi, mà vì niềm vui đoàn tụ...",
    "Hành trình khổ cực đã khép lại bằng một kết thúc trọn vẹn."
    };

    private int currentIndex = 0;
    private Player player;
    private bool isTalking = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.canMove = false; // đứng yên
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            }

            if (dialoguePanel != null)
                dialoguePanel.SetActive(true);

            currentIndex = 0;
            ShowDialogue();
            isTalking = true;
        }
    }

    private void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    void ShowDialogue()
    {
        if (dialogueText != null && currentIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[currentIndex];
        }
    }

    void NextDialogue()
    {
        currentIndex++;
        if (currentIndex < dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        Time.timeScale = 0f; // dừng game
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
