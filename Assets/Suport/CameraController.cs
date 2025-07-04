using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public AudioSource bossBattleMusic;
    private bool inBossBattle = false; // Biến để kiểm tra xem đang trong trạng thái Boss Battle hay không
    public GameObject hpBossCanvas;

    private void Start()
    {
        bossBattleMusic.Stop();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BossBattle"))
        {
            if (!inBossBattle)
            {
                bossBattleMusic.Play();
                hpBossCanvas.SetActive(true);
                inBossBattle = true;
            }
        }
    }
}
