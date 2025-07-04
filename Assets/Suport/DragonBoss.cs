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
    private bool isDead;

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

    [System.Obsolete]
    void Update()
    {
        followPlayer();

        if (currentHPEnemy <= 100000 && !hasTriggeredFireWall)
        {
            hasTriggeredFireWall = true;
            fireWall.SetActive(true); // ✅ chỉ bật fireWall, không liên quan camera nữa
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
            fireWall.SetActive(false);
        }
    }

    void UpdateHP()
    {
        healthSlider.value = (float)currentHPEnemy / maxHP;
        hpBossText.text = $"{currentHPEnemy}/{maxHP}";
    }

    [System.Obsolete]
    private void followPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer > stopRange)
            {
                Vector2 direction = (Player.position - transform.position).normalized;

                // ✅ Di chuyển kiểu top-down
                rb.velocity = direction * 3f;
                animator.SetBool("IsRunning", true);

                // ✅ Flip trái/phải theo hướng X (không xoay người nữa)
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
                rb.velocity = Vector2.zero;
                animator.SetBool("IsRunning", false);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
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
            animator.SetBool("IsIdiel", true);
        }
    }

    [System.Obsolete]
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
                fireBall.GetComponent<Rigidbody2D>().velocity = direction * fireBallSpeed;

                // Flip hướng FireBall
                if (direction.x < 0)
                {
                    fireBall.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    fireBall.transform.localScale = new Vector3(1, 1, 1);
                }

                fireBallTimer = fireBallCooldown;
            }
        }
        else
        {
            animator.SetBool("IsIdiel2", true);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            currentHPEnemy -= 2000;
            swordEffect.Play();
            UpdateHP();

            if (currentHPEnemy <= 0 && !isDead)
            {
                StartCoroutine(DeadEffect());
            }
        }
    }

    private IEnumerator DeadEffect()
    {
        isDead = true;
        deadEffect.Play();
        bloodEffect.Play();
        yield return new WaitForSeconds(1f);
        portalEnd.SetActive(true);
        Destroy(gameObject);
    }
}
