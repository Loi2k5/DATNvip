using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class SkillData
{
    public string skillName;
    public string description;
    public Sprite icon;
    public enum SkillType { DamageUp, SpeedUp, Heal }
    public SkillType type;
}

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpUI;
    public List<SkillData> skillPool; // Các skill có thể xuất hiện

    public Image[] icons;
    public Text[] names;
    public Text[] descriptions;
    public Button[] buttons;

    private List<SkillData> currentOptions = new List<SkillData>();

    public PlayerController player;

    void Start()
    {
        levelUpUI.SetActive(false);
    }

    public void ShowLevelUpUI()
    {
        levelUpUI.SetActive(true);
        Time.timeScale = 0f; // dừng game

        currentOptions.Clear();
        while (currentOptions.Count < 3)
        {
            SkillData random = skillPool[Random.Range(0, skillPool.Count)];
            if (!currentOptions.Contains(random))
                currentOptions.Add(random);
        }

        for (int i = 0; i < 3; i++)
        {
            icons[i].sprite = currentOptions[i].icon;
            names[i].text = currentOptions[i].skillName;
            descriptions[i].text = currentOptions[i].description;

            int index = i;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => SelectSkill(index));
        }
    }

    void SelectSkill(int index)
    {
        ApplySkill(currentOptions[index]);
        levelUpUI.SetActive(false);
        Time.timeScale = 1f; // tiếp tục game
    }

    void ApplySkill(SkillData skill)
    {
        switch (skill.type)
        {
            case SkillData.SkillType.DamageUp:
                player.damage += 5;
                break;
            case SkillData.SkillType.SpeedUp:
                player.moveSpeed += 1;
                break;
            case SkillData.SkillType.Heal:
                player.Heal(20);
                break;
        }
    }
}
