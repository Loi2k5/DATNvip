using UnityEngine;

public class XPOrb : MonoBehaviour
{
    public int xpAmount = 10;
    public float attractionRange = 3f;
    public float moveSpeed = 5f;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attractionRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            XPManager xpManager = Object.FindFirstObjectByType<XPManager>();
            if (xpManager != null)
            {
                xpManager.GainXP(xpAmount);
            }

            Destroy(gameObject);
        }
    }
}
