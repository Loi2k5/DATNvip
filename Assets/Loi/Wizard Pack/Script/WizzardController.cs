using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class WizzardController : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Transform firePoint;                 
    [SerializeField] private GameObject magic1Prefab;             
    [SerializeField] private GameObject magic2ProjectilePrefab;   
    [SerializeField] private AudioClip sfxAttack1;
    [SerializeField] private AudioClip sfxAttack2;

    [Header("Stats")]
    [SerializeField] private float maxHp = 10000f;
    [SerializeField] private float moveSpeed = 3.0f;
    private float currentHp;

    [Header("UI")]
    [SerializeField] private Image hpBar;   // Image với Fill Amount

    [Header("Ranges")]
    [SerializeField] private float detectRange = 50f;
    [SerializeField] private float attackRange = 5f;    
    [SerializeField] private float fireRange = 12f;     

    [Header("Attack Timing")]
    [SerializeField] private float attackCooldown = 3f;   
    [SerializeField] private float spawnDelayA1 = 0.15f;  
    [SerializeField] private float spawnDelayA2 = 0.15f;

    private float nextAttackTime = 0f;

    // Components
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private AudioSource audioSrc;

    // Cache
    private Transform player;
    private Player playerComp;

    // Animator params
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int IsAttack1Hash = Animator.StringToHash("IsAttack1"); 
    private static readonly int IsAttack2Hash = Animator.StringToHash("IsAttack2"); 
    private bool isAttacking = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSrc = GetComponent<AudioSource>();
        if (!audioSrc) audioSrc = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        currentHp = maxHp;
        UpdateHPBar();
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p)
        {
            player = p.transform;
            playerComp = p.GetComponent<Player>();
        }
    }

    private void Update()
    {
        if (player == null) { FindPlayer(); return; }

        float dist = Vector2.Distance(transform.position, player.position);
        sr.flipX = (player.position.x < transform.position.x);

        if (dist > detectRange)
        {
            anim.SetBool(IsRunningHash, false);
            rb.linearVelocity = Vector2.zero;
            return;
        }

        bool canAttack = Time.time >= nextAttackTime;

        if (dist > attackRange && !isAttacking)
        {
            ChaseToPlayer();
        }
        else if (canAttack && !isAttacking)
        {
            if (dist <= attackRange)
                StartCoroutine(DoAttack1());
            else if (dist <= fireRange)
                StartCoroutine(DoAttack2());
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool(IsRunningHash, false);
        }
    }

    private void ChaseToPlayer(float speedMultiplier = 1f)
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * (moveSpeed * speedMultiplier);
        anim.SetBool(IsRunningHash, true);
    }

    private IEnumerator DoAttack1()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool(IsRunningHash, false);
        anim.ResetTrigger(IsAttack2Hash);
        anim.SetTrigger(IsAttack1Hash);

        if (sfxAttack1) audioSrc.PlayOneShot(sfxAttack1);

        yield return new WaitForSeconds(spawnDelayA1);
        if (player) Instantiate(magic1Prefab, player.position, Quaternion.identity);

        nextAttackTime = Time.time + attackCooldown;
        isAttacking = false;
    }

    private IEnumerator DoAttack2()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool(IsRunningHash, false);
        anim.ResetTrigger(IsAttack1Hash);
        anim.SetTrigger(IsAttack2Hash);

        if (sfxAttack2) audioSrc.PlayOneShot(sfxAttack2);

        yield return new WaitForSeconds(spawnDelayA2);

        Vector3 spawnPos = firePoint ? firePoint.position : transform.position;
        GameObject proj = Instantiate(magic2ProjectilePrefab, spawnPos, Quaternion.identity);
        Magic2Projectile m2 = proj.GetComponent<Magic2Projectile>();
        if (m2 != null && player != null) m2.SetTarget(player);

        nextAttackTime = Time.time + attackCooldown;
        isAttacking = false;
    }

    // ————— Damage & Death —————
    public void TakeDamage(float dmg)
    {
        currentHp -= dmg;
        if (currentHp < 0f) currentHp = 0f;
        UpdateHPBar();

        if (currentHp <= 0f)
        {
            if (hpBar != null) Destroy(hpBar.gameObject);
            Destroy(gameObject);
        }
    }

    private void UpdateHPBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerBullet"))
        {
            TakeDamage(150f);
            Destroy(col.gameObject);
        }
    }
}
