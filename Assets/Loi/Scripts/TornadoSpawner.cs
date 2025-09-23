using UnityEngine;

public class TornadoSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tornadoPrefab;
    [SerializeField] private float spawnInterval = 5f; // thời gian spawn ban đầu
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Vector3 spawnOffset;

    private float timer;
    private bool isActive = false;

    private void Update()
    {
        if (!isActive) return;

        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;

            Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position + spawnOffset;
            SpawnTornado(pos);
        }
    }

    private void SpawnTornado(Vector3 spawnPos)
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;

        GameObject tornado = Instantiate(tornadoPrefab, spawnPos, Quaternion.identity);
        tornado.GetComponent<Tornado>().Init(randomDir);
    }

    // Bật/tắt spawner
    public void SetActive(bool active)
    {
        isActive = active;
        timer = 0f; // reset timer khi bật lại
    }

    // Giảm thời gian spawn
    public void DecreaseSpawnInterval(float amount, float minLimit = 1f)
    {
        spawnInterval = Mathf.Max(minLimit, spawnInterval - amount);
    }
}
