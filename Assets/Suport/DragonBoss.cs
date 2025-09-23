using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DragonBoss : MonoBehaviour
{
    public float detectionRangeAttack = 2.5f;
    public float detectionRange = 10f;
    public float fireBallRange = 20f;
    public float fireBallSpeed = 5f;
    private float stopRange = 0.5f;

    public Transform Player;
    public Transform Player2;
    private float TimeAttackRate = 2f;
    private float timeAttack;

    public GameObject portalEnd;
    public GameObject tuong;
    private bool right = true;

    public Slider healthSlider;
    public int health;
    public int currentHPEnemy;
    public int maxHP;

    Animator animator;
    Rigidbody2D rb;

    public Transform Knifedamage;
    public GameObject hitbox;
    public ParticleSystem deadEffect;
    public ParticleSystem bloodEffect;
    public ParticleSystem swordEffect;
    [Header("Public để pet kiểm tra")]
    public bool isDead = false;

    public TextMeshProUGUI hpBossText;
    public AudioSource dragonBossAudio;

    public GameObject fireBallPrefab;
    public Transform firePoint;
    private float fireBallCooldown = 2f;
    private float fireBallTimer;

    public GameObject fireWall;
    private bool hasTriggeredFireWall = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHPEnemy = health;
        UpdateHP();
        fireBallTimer = fireBallCooldown;
    }

    void Update()
    {
        if (isDead) return;

        followPlayer();

        if (currentHPEnemy <= 100000 && !hasTriggeredFireWall)
        {
            hasTriggeredFireWall = true;
            if (fireWall != null) fireWall.SetActive(true);
        }

        if (currentHPEnemy <= 100000)
        {
            FlameAttack();
        }
        else if (currentHPEnemy <= 200000)
        {
            NormalAttack();
        }

        if (currentHPEnemy <= 0)
        {
            dragonBossAudio.Stop();
            if (fireWall != null) fireWall.SetActive(false);
        }
    }

    public void UpdateHP()
    {
        healthSlider.value = (float)currentHPEnemy / maxHP;
        hpBossText.text = $"{currentHPEnemy}/{maxHP}";
    }

    private void followPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > stopRange)
            {
                Vector2 direction = (Player.position - transform.position).normalized;
                rb.linearVelocity = direction * 3f;
                animator.SetBool("IsRunning", true);

                if ((direction.x < 0 && right) || (direction.x > 0 && !right))
                {
                    right = !right;
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                }
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsRunning", false);
        }
    }

    void NormalAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        if (distanceToPlayer < detectionRangeAttack)
        {
            timeAttack -= Time.deltaTime;

            if (timeAttack <= 0)
            {
                animator.SetTrigger("IsNormalAttack");
                var oneSkill = Instantiate(hitbox, Knifedamage.position, Quaternion.identity);
                Destroy(oneSkill, 0.1f);
                timeAttack = TimeAttackRate;
            }
        }
        else
        {
            animator.SetBool("IsIdle", true); // sửa chính tả: IsIdiel → IsIdle
        }
    }

    void FlameAttack()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.position);
        if (distanceToPlayer <= fireBallRange)
        {
            fireBallTimer -= Time.deltaTime;

            if (fireBallTimer <= 0)
            {
                animator.SetTrigger("IsFlameAttack");
                var fireBall = Instantiate(fireBallPrefab, firePoint.position, Quaternion.identity);

                Vector2 direction = (Player.position - firePoint.position).normalized;
                fireBall.GetComponent<Rigidbody2D>().linearVelocity = direction * fireBallSpeed;

                // Flip fireball
                fireBall.transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);

                fireBallTimer = fireBallCooldown;
            }
        }
        else
        {
            animator.SetBool("IsIdle2", true); // sửa chính tả: IsIdiel2 → IsIdle2
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            TakeDamage(2000);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHPEnemy -= amount;
        if (currentHPEnemy < 0) currentHPEnemy = 0;

        swordEffect.Play();
        UpdateHP();

        if (currentHPEnemy <= 0 && !isDead)
        {
            StartCoroutine(DeadEffect());
        }

        Debug.Log("🔥 Boss bị mất máu! Còn lại: " + currentHPEnemy);
    }

    private IEnumerator DeadEffect()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetTrigger("Die");
        deadEffect.Play();
        bloodEffect.Play();

<<<<<<< HEAD
        // Ẩn thanh máu và text máu
        healthSlider.gameObject.SetActive(false);
        hpBossText.gameObject.SetActive(false);

        yield return new WaitForSeconds(1f);
        portalEnd.SetActive(true);
        tuong.SetActive(false);
=======
        yield return new WaitForSeconds(1.5f); // đủ thời gian cho animation

        if (portalEnd != null)
        {
            portalEnd.SetActive(true);
        }

>>>>>>> long
        Destroy(gameObject);
    }
}
