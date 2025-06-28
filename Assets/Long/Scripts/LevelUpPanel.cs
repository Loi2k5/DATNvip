using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LevelUpPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private SkillButton[] skillButtons; // mảng 3 nút

    [SerializeField] private Skill[] allSkills; // Danh sách kỹ năng gốc

    private System.Random random = new System.Random();

    public void ShowAvailableUpgrades()
    {
        panel.SetActive(true);

        Skill[] selectedSkills = GetRandomSkills(3);

        for (int i = 0; i < skillButtons.Length; i++)
        {
            skillButtons[i].Setup(selectedSkills[i], OnSkillChosen);
        }
    }

    private void OnSkillChosen(Skill chosenSkill)
    {
        chosenSkill.Apply();
        panel.SetActive(false);
        FindObjectOfType<GameManager>().ResetEnergy(); // tiếp tục game
    }

    private Skill[] GetRandomSkills(int count)
    {
        Skill[] result = new Skill[count];
        var shuffled = allSkills.OrderBy(s => random.Next()).ToArray();

        for (int i = 0; i < count; i++)
            result[i] = shuffled[i];

        return result;
    }
}
