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
    public float dashCooldown = 3f; // thời gian hồi chiêu
    private float cooldownTimer = 0f;
    public TextMeshProUGUI dashCooldownText;   // UI Text hiển thị cooldown

    public bool canMove = true; // Cho phép di chuyển hay không

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
    }

    void Update()
    {
        if (!canMove || isDead) return; // Nếu bị khóa thì bỏ qua mọi input
        HandleDash();
        // Tính toán cooldown dash
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

        // Tự động trừ máu nếu đang đứng trong vùng FireWall
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
            cooldownTimer = dashCooldown; // bắt đầu hồi chiêu
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

    void MovePlayer()
    {
        if (!canMove) return; // Không cho di chuyển khi bị khóa
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
        if (currentHp <= 0)
        {
            Die();
        }
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
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }

    public void TangTocChay()
    {
        moveSpeed = Mathf.Min(moveSpeed + 2f, maxMoveSpeed);
    }

    public void HoiMaul()
    {
        maxHp = Mathf.Min(maxHp + 300f, 2900f);
        currentHp = maxHp;
        UpdateHpBar();
    }

    // --- Va chạm sát thương ---
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
            fireWallTimer = 0f; // reset ngay
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FireWall"))
        {
            isInFireWall = false;
        }
    }
    // Giảm hồi chiêu dash
    public void ReduceDashCooldown(float amount)
    {
        dashCooldown = Mathf.Max(0.5f, dashCooldown - amount); // giới hạn không < 0.5s
    }

    // Tăng máu tối đa
    public void IncreaseMaxHp(float amount)
    {
        maxHp += amount;
        currentHp = maxHp;
        UpdateHpBar();
    }
}
