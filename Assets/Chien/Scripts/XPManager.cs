using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPManager : MonoBehaviour
{
    public Slider xpSlider;         
    public TMP_Text levelText;      

    public int currentXP = 0;
    public int maxXP = 100;
    public int currentLevel = 1;

    void Start()
    {
        xpSlider.maxValue = maxXP;
        xpSlider.value = currentXP;
        UpdateLevelText();
    }

    public void GainXP(int amount)
    {
        if (currentLevel >= 99)
        {
            currentXP = maxXP;
            xpSlider.value = currentXP;
            return;
        }

        currentXP += amount;

        while (currentXP >= maxXP && currentLevel < 99)
        {
            currentXP -= maxXP;
            currentLevel++;

            maxXP = Mathf.RoundToInt(maxXP * 1.05f);

            xpSlider.maxValue = maxXP;
            UpdateLevelText();
        }

        xpSlider.value = currentXP;
    }

    private void UpdateLevelText()
    {
        levelText.text = "Level " + currentLevel;
    }

    public void TestAddXP()
    {
        GainXP(20); // tăng 20 XP mỗi lần bấm
    }
}
