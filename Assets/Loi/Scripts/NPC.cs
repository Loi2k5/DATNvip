using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    [TextArea] public string[] dialogueLines;
    public float typingSpeed = 0.03f;

    public GameObject enemySpawner;
    public GameObject chatIcon;

    private int currentLineIndex = 0;
    private bool isTalking = false;
    private bool isTyping = false;
    private bool hasTalked = false; // Đánh dấu đã nói chuyện chưa
    private Player player;

    void Start()
    {
        dialogueUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTalking && !hasTalked) // Chỉ bắt đầu nếu chưa nói chuyện
        {
            player = other.GetComponent<Player>();
            if (player != null)
            {
                player.canMove = false;
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            }

            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogueUI.SetActive(true);
        currentLineIndex = 0;
        StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (isTalking && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    void NextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    System.Collections.IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in dialogueLines[currentLineIndex].ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueUI.SetActive(false);
        isTalking = false;
        hasTalked = true; // Đánh dấu đã trò chuyện

        if (player != null)
            player.canMove = true;

        if (enemySpawner != null)
            enemySpawner.SetActive(true);

        if (chatIcon != null)
            chatIcon.SetActive(false);
    }
}
