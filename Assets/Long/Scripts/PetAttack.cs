using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class PetAI : MonoBehaviour
{
    public Transform player;
    public float followDistance = 2f;
    public float moveSpeed = 3f;

    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float damage = 100f;
    public float attackCooldown = 1f;

    private Transform targetEnemy;
    private float lastAttackTime;

    private Rigidbody2D rb;
    private Animator animator;

    public GameObject hitEffect;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        FindClosestEnemyOrBoss();

        if (targetEnemy != null && Vector2.Distance(transform.position, targetEnemy.position) <= attackRange)
        {
            AttackTarget();
        }
        else
        {
            FollowPlayer();
        }

        FlipDirection();
    }

    void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > followDistance)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            rb.linearVelocity = dir * moveSpeed;
            animator.SetBool("isRunning", true);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isRunning", false);
        }
    }

    void FindClosestEnemyOrBoss()
    {
        float closestDistance = Mathf.Infinity;
        targetEnemy = null;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        foreach (var hit in hits)
        {
            if (hit.gameObject == this.gameObject) continue;

            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist >= closestDistance) continue;

            var scripts = hit.GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                // Kiểm tra có method TakeDamage không
                MethodInfo takeDamage = script.GetType().GetMethod("TakeDamage", BindingFlags.Public | BindingFlags.Instance);

                // Kiểm tra có biến isDead không?
                FieldInfo isDeadField = script.GetType().GetField("isDead", BindingFlags.Public | BindingFlags.Instance);

                bool isDead = false;
                if (isDeadField != null)
                {
                    isDead = (bool)isDeadField.GetValue(script);
                }

                if (takeDamage != null && !isDead)
                {
                    closestDistance = dist;
                    targetEnemy = hit.transform;
                    break;
                }
            }
        }
    }

    void AttackTarget()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        if (targetEnemy == null) return;

        lastAttackTime = Time.time;
        rb.linearVelocity = Vector2.zero;

        if (animator != null)
            animator.SetTrigger("Attack");

        if (hitEffect != null)
            Instantiate(hitEffect, targetEnemy.position, Quaternion.identity);

        var scripts = targetEnemy.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            MethodInfo method = script.GetType().GetMethod("TakeDamage", BindingFlags.Public | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(script, new object[] { (int)damage });
                break;
            }
        }
    }

    void FlipDirection()
    {
        Vector3 scale = transform.localScale;
        if (targetEnemy != null)
        {
            scale.x = targetEnemy.position.x < transform.position.x ? -1 : 1;
        }
        else if (player != null)
        {
            scale.x = player.position.x < transform.position.x ? -1 : 1;
        }
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
