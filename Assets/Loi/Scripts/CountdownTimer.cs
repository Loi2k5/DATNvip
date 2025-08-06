using UnityEngine;
using TMPro;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;    // UI text hiển thị thời gian
    public float timeRemaining = 120f; // 2 phút
    private bool timerRunning = true;

    public GameObject bossPrefab;
    public Transform bossSpawnPoint;
    public GameObject enemySpawner; // script hoặc object điều khiển spawn quái

    void Update()
    {
        if (timerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerRunning = false;
                UpdateTimerUI(timeRemaining);
                OnTimerEnd();
            }
        }
    }

    void UpdateTimerUI(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"<color=red>{minutes:00}:{seconds:00}</color>";
    }

    void OnTimerEnd()
    {
        if (enemySpawner != null)
        {
            enemySpawner.SetActive(false); // Dừng spawn quái
        }

        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, Quaternion.identity);
        }
    }
}
