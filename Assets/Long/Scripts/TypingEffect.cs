using UnityEngine;
using TMPro;
using System.Collections;

public class TypingEffect : MonoBehaviour
{
    [Header("UI Component")]
    public TextMeshProUGUI storyText;   // Gắn TextMeshProUGUI (StoryText)

    [Header("Story Settings")]
    [TextArea(5, 15)]
    public string fullText;             // Nội dung story
    public float typingSpeed = 0.04f;   // Thời gian delay mỗi ký tự

    private Coroutine typingCoroutine;

    void Start()
    {
        // Tự động chạy khi scene bắt đầu
        PlayStory(fullText);
    }

    public void PlayStory(string text)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(text));
    }

    IEnumerator TypeText(string text)
    {
        storyText.text = "";
        foreach (char c in text)
        {
            storyText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
