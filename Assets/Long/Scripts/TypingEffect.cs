using System.Collections;
using UnityEngine;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    [TextArea(5, 20)] public string fullText;
    public float typingSpeed = 0.04f;

    public GameObject storyImage; // Ảnh nền minh họa

    private bool isTyping = false;

    private void Start()
    {
        targetText.text = "";
        storyImage.SetActive(true); // Cho ảnh hiện ngay từ đầu
        StartTyping(); // Gọi typing ngay khi bắt đầu
    }

    public void StartTyping()
    {
        StopAllCoroutines();
        targetText.text = "";
        storyImage.SetActive(true);
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        isTyping = true;
        string current = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            current += fullText[i];
            targetText.text = current;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}
