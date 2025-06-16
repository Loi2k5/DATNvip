using UnityEngine;

public class EnemyKillTracker : MonoBehaviour
{
    public int kills = 0;
    public int killsToLevelUp = 10;
    public LevelUpManager levelUpManager;

    public void AddKill()
    {
        kills++;
        if (kills >= killsToLevelUp)
        {
            kills = 0;
            levelUpManager.ShowLevelUpUI();
        }
    }
}
