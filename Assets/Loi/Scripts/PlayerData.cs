using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance; // Singleton để dễ truy cập
    public int currentPoints;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại qua các scene
        }
        else
        {
            Destroy(gameObject); // Tránh duplicate khi trở về scene cũ
        }
    }
}