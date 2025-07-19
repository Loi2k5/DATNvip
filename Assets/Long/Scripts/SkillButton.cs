using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Button button;

    private Skill skill;
    private System.Action<Skill> callback;

    public void Setup(Skill skillData, System.Action<Skill> onClick)
    {
        skill = skillData;
        callback = onClick;

        if (icon) icon.sprite = skill.icon;
        if (title) title.text = skill.skillName;
        if (description) description.text = skill.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => callback?.Invoke(skill));
    }
}
