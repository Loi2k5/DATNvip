using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    [TextArea(5, 20)] public string fullText;
    public float typingSpeed = 0.04f;

    public GameObject storyImage; // ← ảnh nền minh họa

    private void Start()
    {
        targetText.text = "";
        storyImage.SetActive(true); // ← Cho ảnh hiện ngay từ đầu
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        string current = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            current += fullText[i];
            targetText.text = current;

            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
