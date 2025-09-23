using UnityEngine;

public class MentorSpawn : MonoBehaviour
{
    public static MentorSpawn Instance;

    [Header("Thiên thạch")]
    [SerializeField] private GameObject meteorPrefab;   // Prefab thiên thạch
    [SerializeField] private float meteorInterval = 5f; // 5 giây rơi 1 lần
    [SerializeField] private float spawnHeight = 10f;   // spawn trên cao
    [SerializeField] private GameObject panelToOpen;

    private float timer;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= meteorInterval)
        {
            timer = 0f;
            SpawnMeteorOnEnemy();
        }
    }

    private void SpawnMeteorOnEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) return;

        // Chọn ngẫu nhiên 1 enemy
        GameObject target = enemies[Random.Range(0, enemies.Length)];

        if (target != null)
        {
            Vector3 spawnPos = target.transform.position + Vector3.up * spawnHeight;
            GameObject meteor = Instantiate(meteorPrefab, spawnPos, meteorPrefab.transform.rotation);

            Meteor meteorScript = meteor.GetComponent<Meteor>();
            if (meteorScript != null)
            {
                meteorScript.SetTarget(target.transform.position);
            }
        }
    }
    public void OpenPanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true);  // Mở panel
        }
    }
}
