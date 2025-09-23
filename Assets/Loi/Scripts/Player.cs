using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private float maxMoveSpeed = 10f;
    [SerializeField] private GameManager gameManager;
    private bool isDead = false;

    // --- FireWall damage ---
    private bool isInFireWall = false;
    private float fireWallDamageInterval = 1f;
    private float fireWallTimer = 0f;

    // --- Dash ---
    public float dashBoost = 20f;
    public float dashTime = 0.2f;
    private float dashTimer;
    private bool isDashing = false;

    [Header("Dash Cooldown")]
    public float dashCooldown = 3f;
    private float cooldownTimer = 0f;
    public TextMeshProUGUI dashCooldownText;

    public bool canMove = true;

    // --- Heal Skill ---
    [Header("Heal Skill")]
    [SerializeField] private float healCooldown = 15f;
    private float healTimer = 0f;
    [SerializeField] private Image healSkillIcon;          // UI icon kỹ năng
    [SerializeField] private TextMeshProUGUI healCooldownText; // UI text
    [SerializeField] private ParticleSystem healVFX;       // Hiệu ứng hồi máu
    private bool healReady = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();

        if (dashCooldownText != null)
            dashCooldownText.gameObject.SetActive(false);

        if (healCooldownText != null)
            healCooldownText.text = "Sẵn sàng";

        if (healSkillIcon != null)
            healSkillIcon.color = Color.white;

        if (healVFX != null)
            healVFX.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!canMove || isDead) return;
        HandleDash();
        HandleHealSkill();

        // Dash cooldown hiển thị
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            if (dashCooldownText != null)
            {
                dashCooldownText.gameObject.SetActive(true);
                dashCooldownText.text = Mathf.Ceil(cooldownTimer).ToString();
            }
        }
        else
        {
            if (dashCooldownText != null)
                dashCooldownText.gameObject.SetActive(false);
        }

        if (isDead) return;

        MovePlayer();

        if (isInFireWall)
        {
            fireWallTimer += Time.deltaTime;
            if (fireWallTimer >= fireWallDamageInterval)
            {
                TakeDamage(2f);
                fireWallTimer = 0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGameMenu();
        }
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && cooldownTimer <= 0)
        {
            moveSpeed += dashBoost;
            dashTimer = dashTime;
            isDashing = true;
            cooldownTimer = dashCooldown;
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                moveSpeed -= dashBoost;
                isDashing = false;
            }
        }
    }

    // --- Heal Skill logic ---
    void HandleHealSkill()
    {
        if (!healReady)
        {
            healTimer -= Time.deltaTime;
            if (healCooldownText != null)
                healCooldownText.text = Mathf.Ceil(healTimer).ToString();

            if (healTimer <= 0)
            {
                healReady = true;
                if (healCooldownText != null) healCooldownText.text = "Sẵn sàng";
                if (healSkillIcon != null) healSkillIcon.color = Color.white;
            }
        }

        // Phím Q để hồi máu
        if (Input.GetKeyDown(KeyCode.Q) && healReady && !isDead)
        {
            ActivateHealSkill();
        }
    }

    void ActivateHealSkill()
    {
        healReady = false;
        healTimer = healCooldown;

        // Hồi 50% máu tối đa
        float healValue = maxHp * 0.5f;
        currentHp = Mathf.Min(currentHp + healValue, maxHp);
        UpdateHpBar();

        // Hiệu ứng
        if (healVFX != null)
            StartCoroutine(PlayHealVFX());

        // UI chuyển sang cooldown
        if (healSkillIcon != null) healSkillIcon.color = Color.gray;
    }

    private System.Collections.IEnumerator PlayHealVFX()
    {
        healVFX.gameObject.SetActive(true);
        healVFX.Play();
        yield return new WaitForSeconds(2f);
        healVFX.Stop();
        healVFX.gameObject.SetActive(false);
    }

    void MovePlayer()
    {
        if (!canMove) return;
        Vector2 playeInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playeInput.normalized * moveSpeed;

        spriteRenderer.flipX = playeInput.x < 0;
        animator.SetBool("isRun", playeInput != Vector2.zero);
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0) Die();
    }

    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHpBar();
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetBool("isRun", false);
        rb.linearVelocity = Vector2.zero;
        Time.timeScale = 0f;
        gameManager.GameOverMenu();
    }

    private void UpdateHpBar()
    {
        if (hpBar != null)
            hpBar.fillAmount = currentHp / maxHp;
    }

    // --- FireWall enter/exit ---
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("BossSword"))
        {
            TakeDamage(50f);
        }
        else if (collision.CompareTag("FireBallDragonBall"))
        {
            TakeDamage(100f);
        }
        else if (collision.CompareTag("FireWall"))
        {
            isInFireWall = true;
            fireWallTimer = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FireWall"))
        {
            isInFireWall = false;
        }
    }

    public void ReduceDashCooldown(float amount)
    {
        dashCooldown = Mathf.Max(0.5f, dashCooldown - amount);
    }

    public void IncreaseMaxHp(float amount)
    {
        maxHp += amount;
        currentHp = maxHp;
        UpdateHpBar();
    }
}
