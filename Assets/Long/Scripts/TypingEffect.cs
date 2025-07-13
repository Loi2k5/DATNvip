using System.Collections;
using UnityEngine;
using TMPro;

public class TypingEffect : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    [TextArea(5, 20)] public string fullText;
    public float typingSpeed = 0.04f;

    private void Start()
    {
        targetText.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        string current = "";

        foreach (char c in fullText)
        {
            current += c;
            targetText.text = current;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
