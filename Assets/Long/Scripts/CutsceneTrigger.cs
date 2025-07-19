using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public GameObject momShin;
    public GameObject dadShin;
    public DialogueUI dialogueUI;

    private bool triggered = false;

    void Update()
    {
        // Khi cả bố và mẹ đều hiện (hoặc điều kiện riêng của bạn)
        if (!triggered && momShin.activeInHierarchy && dadShin.activeInHierarchy)
        {
            dialogueUI.ShowDialogue("Shin! Chúng ta là bố mẹ con đây! Tự hào về con!");
            triggered = true;
        }
    }
}
