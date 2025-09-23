using UnityEngine;
using TMPro;
using System.Collections;

public class NPCDialogueTriggerWithStory : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public GameObject storyText;

    [TextArea(2, 5)]
    public string message = "Shin!, con trai của chúng ta đã trưởng thành rồi!";
    public float typingSpeed = 0.04f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool storyShown = false;

    void Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (storyText != null) storyText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialogue());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        if (dialogueText != null)
            dialogueText.text = "";

        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        if (storyText != null)
            storyText.SetActive(false);

        isTyping = false;
        storyShown = false;
    }

    IEnumerator TypeDialogue()
    {
        if (dialogueText == null) yield break;

        dialogueText.text = "";
        isTyping = true;

        foreach (char letter in message)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        ShowStory();
    }

    void ShowStory()
    {
        if (!storyShown && storyText != null)
        {
            storyText.SetActive(true);
            storyShown = true;
        }
    }

    void Update()
    {
        // Nhấn Space để tắt storyText nếu đang hiện
        if (storyShown && Input.GetKeyDown(KeyCode.Space))
        {
            storyText.SetActive(false);
        }
    }
}
