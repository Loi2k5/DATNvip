using UnityEngine;

public class SkillSelector : MonoBehaviour
{
    [Header("Star Groups for Each Skill")]
    [SerializeField] private GameObject[] starsSkill1; // 3 sao cho K1
    [SerializeField] private GameObject[] starsSkill2; // 3 sao cho K2
    [SerializeField] private GameObject[] starsSkill3; // 3 sao cho K3

    private int levelSkill1 = 0;
    private int levelSkill2 = 0;
    private int levelSkill3 = 0;

    public void SelectSkill1()
    {
        if (levelSkill1 < 3)
        {
            levelSkill1++;
            UpdateStars(starsSkill1, levelSkill1);
            // Gọi nâng cấp thực tế, ví dụ: FindObjectOfType<Gun>().TangTocBan();
        }
    }

    public void SelectSkill2()
    {
        if (levelSkill2 < 3)
        {
            levelSkill2++;
            UpdateStars(starsSkill2, levelSkill2);
        }
    }

    public void SelectSkill3()
    {
        if (levelSkill3 < 3)
        {
            levelSkill3++;
            UpdateStars(starsSkill3, levelSkill3);
        }
    }

    private void UpdateStars(GameObject[] stars, int level)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].SetActive(i < level);
        }
    }

    private void Start()
    {
        // Đảm bảo sao ban đầu tắt hết
        UpdateStars(starsSkill1, levelSkill1);
        UpdateStars(starsSkill2, levelSkill2);
        UpdateStars(starsSkill3, levelSkill3);
    }
}
