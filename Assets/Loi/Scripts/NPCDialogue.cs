using UnityEngine;
using TMPro;

public class NPCDialogue : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    [TextArea] public string[] dialogueLines;
    public float typingSpeed = 0.03f;

    private int currentLineIndex = 0;
    private bool isTalking = false;
    private bool isTyping = false;
    private Player player;

    void Start()
    {
        dialogueUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTalking)
        {
            player = other.GetComponent<Player>();
            if (player != null) player.canMove = false; // Khóa điều khiển

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

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Dừng trong Editor
#else
        Application.Quit(); // Thoát khi build
#endif
    }
}
