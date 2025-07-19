using UnityEngine;
using UnityEngine.UI;

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

    public float dashBoost;
    public float dashTime;
    private float _dashTime;
    bool isDashing = false;

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
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && _dashTime<=0 && isDashing ==false)
        {
            moveSpeed += dashBoost;
            _dashTime = dashTime;
            isDashing=true;
        }
        if(_dashTime <=0 && isDashing ==true)
        {
            moveSpeed -= dashBoost;
            isDashing = false;
        }
        else
        {
            _dashTime -=Time.deltaTime;
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

    void MovePlayer()
    {
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
}
