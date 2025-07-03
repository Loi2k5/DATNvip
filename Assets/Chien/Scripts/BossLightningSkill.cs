using System.Collections;
using UnityEngine;

public class BossLightningSkill : MonoBehaviour
{
    public GameObject lightningPrefab; // Prefab hiệu ứng sét
    public int strikeCount = 5;
    public float strikeInterval = 1f;
    public float cooldownTime = 10f;
    public float skillRange = 5f;

    private bool canUseSkill = true;
    private Transform player;

    void Start()
    {
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
            player = foundPlayer.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= skillRange && canUseSkill)
        {
            StartCoroutine(ActivateLightningSkill());
        }
    }

    IEnumerator ActivateLightningSkill()
    {
        canUseSkill = false;

        for (int i = 0; i < strikeCount; i++)
        {
            if (player == null) break;

            // Chỉ tạo hiệu ứng sét, không gây damage ở đây
            Vector3 spawnPosition = player.position;
            Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);

            yield return new WaitForSeconds(strikeInterval);
        }

        yield return new WaitForSeconds(cooldownTime);
        canUseSkill = true;
    }
}
