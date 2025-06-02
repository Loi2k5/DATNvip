using UnityEngine;
using System.Collections;

public class HealingEnemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public float healAmount = 5f;
    public float healInterval = 2f;
    public float moveSpeed = 2f;
    public float detectionRange = 5f;

    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(HealOverTime());
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        isChasing = distanceToPlayer <= detectionRange;

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    IEnumerator HealOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(healInterval);
            if (currentHealth < maxHealth)
            {
                currentHealth += healAmount;
                currentHealth = Mathf.Min(currentHealth, maxHealth);
                Debug.Log("Quái hồi máu: " + currentHealth);
            }
        }
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }
}
