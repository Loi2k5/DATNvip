using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Cinemachine; // Cinemachine 3.x

public class DragonBoss : MonoBehaviour
{
    [Header("Movement & Attack")]
    public float detectionRangeAttack = 2.5f;
    public float detectionRange = 10f;
    public float fireBallRange = 20f;
    public float fireBallSpeed = 5f;
    private float stopRange = 0.5f;
    private float TimeAttackRate = 2f;
    private float timeAttack;

    [Header("Refs")]
    public Transform Player;
    public Transform Player2; // giữ nguyên nếu bạn dùng nơi khác
    public GameObject portalEnd;
    public GameObject tuong;
    public Transform Knifedamage;
    public GameObject hitbox;
    public ParticleSystem deadEffect;
    public ParticleSystem bloodEffect;
    public ParticleSystem swordEffect;
    public GameObject fireBallPrefab;
    public Transform firePoint;
    public GameObject fireWall;

    [Header("Summon Wizzard (Phase 1)")]
    public Transform portalPos1;
    public Transform portalPos2;
    public GameObject portalPrefab;
    public GameObject wizzardPrefab;

    [Header("Camera (Cinemachine 3.x)")]
    public CinemachineCamera playerCam;
    public CinemachineCamera bossCam;

    [Header("UI/HP")]
    public Slider healthSlider;
    public TextMeshProUGUI hpBossText;
    public int health;
    public int currentHPEnemy;
    public int maxHP;

    [Header("Mid-Phase Cutscene UI")]
    [SerializeField] private GameObject panelBossMessage;     // Panel hiển thị thoại (inactive mặc định)
    [SerializeField] private TextMeshProUGUI bossMessageTMP;  // TMP text trong panel

    [Header("Audio")]
    public AudioSource dragonBossAudio;   // Chỉ có AudioSource, không gắn clip sẵn
    public AudioClip BossPhase1;          // intro, không loop
    public AudioClip BossPhase1Loop;      // loop cho đến khi còn 50% HP
    public AudioClip BossMidPhase1;       // mid, boss bất tử
    public AudioClip BossLastPhase;       // loop cho đến khi chết

    private Animator animator;
    private Rigidbody2D rb;
    private bool right = true;
    private bool isDead;
    private bool isInvulnerable = false;      // bất tử trong MidPhase
    private bool hasTriggeredFireWall = false;

    private float fireBallCooldown = 2f;
    private float fireBallTimer;

    // Audio/phase state
    private bool musicStarted = false;
    private bool midPhaseStarted = false;
    private bool lastPhaseStarted = false;

    private enum BossPhase { Phase1, MidTransition, Phase2 }
    private BossPhase phase = BossPhase.Phase1;

    // Summon state
    private bool hasSummoned = false;
    private bool isSummoning = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        currentHPEnemy = health;
        UpdateHP();

        fireBallTimer = fireBallCooldown;

        if (!hasSummoned)
        {
            StartCoroutine(SummonWizzardPhase1());
            hasSummoned = true;
        }

        if (!musicStarted)
        {
            StartCoroutine(AudioDirector());
            musicStarted = true;
        }

        if (panelBossMessage) panelBossMessage.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        // Boss đứng yên khi đang summon
        if (isSummoning)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("IsRunning", false);
            return;
        }

        // Bật FireWall một lần khi HP dưới mốc
        if (currentHPEnemy <= 100000 && !hasTriggeredFireWall)
        {
            hasTriggeredFireWall = true;
            fireWall.SetActive(true);
        }

        // Phase2: nếu Player chạy xa >7f thì teleport lại
        if (phase == BossPhase.Phase2 && Player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, Player.position);
            if (distToPlayer > 7f) TeleportCloseToPlayer();
        }

        // Theo dõi & tấn công khi không ở mid transition
        if (phase != BossPhase.MidTransition)
        {
            followPlayer();

            if (currentHPEnemy <= 100000)       FlameAttack();
            else if (currentHPEnemy <= 200000)  NormalAttack();
        }

        if (currentHPEnemy <= 0)
        {
            if (dragonBossAudio.isPlaying) dragonBossAudio.Stop();
            fireWall.SetActive(false);
        }
    }

    // ---------- CAMERA BLEND ----------
    private void FocusBossCam(bool focusBoss)
    {
        if (!playerCam || !bossCam) return;
        bossCam.Priority = focusBoss ? 20 : 10;
        playerCam.Priority = focusBoss ? 10 : 20; // CinemachineBrain sẽ blend tự động
    }

    // ---------- SUMMON SEQUENCE ----------
    private IEnumerator SummonWizzardPhase1()
    {
        isSummoning = true;
        FocusBossCam(true);               // chuyển cam sang Boss

        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsRunning", false);

        // Đợi 2s loading
        yield return new WaitForSeconds(2f);

        // Spawn 2 cổng
        GameObject portal1 = null, portal2 = null;
        if (portalPrefab)
        {
            portal1 = Instantiate(portalPrefab, portalPos1.position, Quaternion.identity);
            portal2 = Instantiate(portalPrefab, portalPos2.position, Quaternion.identity);
        }

        // Sau 1s mới gọi Wizzard (để thấy cổng trước)
        yield return new WaitForSeconds(1f);

        GameObject wiz1 = null, wiz2 = null;
        if (wizzardPrefab)
        {
            wiz1 = Instantiate(wizzardPrefab, portalPos1.position, Quaternion.identity);
            wiz2 = Instantiate(wizzardPrefab, portalPos2.position, Quaternion.identity);

            ToggleWizzardAI(wiz1, false);
            ToggleWizzardAI(wiz2, false);
            FreezeWizardPhysics(wiz1, true);
            FreezeWizardPhysics(wiz2, true);

            // Fade-in 1s
            SetAlphaRecursive(wiz1, 0f);
            SetAlphaRecursive(wiz2, 0f);
            float t = 0f, fadeTime = 1f;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                float a = Mathf.Lerp(0f, 1f, t / fadeTime);
                SetAlphaRecursive(wiz1, a);
                SetAlphaRecursive(wiz2, a);
                yield return null;
            }
        }

        // Cổng tổng 5s (đã chờ 1s trước) → chờ thêm 4s
        yield return new WaitForSeconds(4f);

        if (portal1) Destroy(portal1);
        if (portal2) Destroy(portal2);
        FreezeWizardPhysics(wiz1, false);
        FreezeWizardPhysics(wiz2, false);
        ToggleWizzardAI(wiz1, true);
        ToggleWizzardAI(wiz2, true);

        FocusBossCam(false);              // trả camera về Player
        isSummoning = false;
    }

    private void ToggleWizzardAI(GameObject wiz, bool enable)
    {
        if (!wiz) return;
        var wizCtrl = wiz.GetComponent<WizzardController>();
        if (wizCtrl) wizCtrl.enabled = enable;
        else foreach (var mb in wiz.GetComponents<MonoBehaviour>()) mb.enabled = enable;

        var rb2 = wiz.GetComponent<Rigidbody2D>();
        if (rb2) rb2.linearVelocity = Vector2.zero;
    }

    private void FreezeWizardPhysics(GameObject wiz, bool freeze)
    {
        if (!wiz) return;
        var rb2 = wiz.GetComponent<Rigidbody2D>();
        if (rb2)
        {
            if (freeze) { rb2.linearVelocity = Vector2.zero; rb2.simulated = false; }
            else rb2.simulated = true;
        }
    }

    private void SetAlphaRecursive(GameObject obj, float alpha)
    {
        if (!obj) return;
        var srs = obj.GetComponentsInChildren<SpriteRenderer>(true);
        foreach (var s in srs)
        {
            var c = s.color; c.a = alpha; s.color = c;
        }
    }

    // ---------- HP / UI ----------
    void UpdateHP()
    {
        healthSlider.value = (float)currentHPEnemy / maxHP;
        hpBossText.text = $"{currentHPEnemy}/{maxHP}";
    }

    // ---------- MOVE / ATTACK ----------
    private void followPlayer()
    {
        if (!Player) return;
        float d = Vector2.Distance(transform.position, Player.position);

        if (d <= detectionRange)
        {
            if (d > stopRange)
            {
                Vector2 dir = (Player.position - transform.position).normalized;
                rb.linearVelocity = dir * 3f;
                animator.SetBool("IsRunning", true);

                if ((dir.x < 0 && right) || (dir.x > 0 && !right))
                {
                    right = !right;
                    var sc = transform.localScale; sc.x *= -1; transform.localScale = sc;
                }
            }
            else { rb.linearVelocity = Vector2.zero; animator.SetBool("IsRunning", false); }
        }
        else { rb.linearVelocity = Vector2.zero; animator.SetBool("IsRunning", false); }
    }

    void NormalAttack()
    {
        if (!Player) return;
        float d = Vector3.Distance(transform.position, Player.position);
        if (d < detectionRangeAttack)
        {
            timeAttack -= Time.deltaTime;
            if (timeAttack <= 0f)
            {
                animator.SetTrigger("IsNormalAttack");
                var oneSkill = Instantiate(hitbox, Knifedamage.position, Quaternion.identity);
                Destroy(oneSkill, 0.1f);
                timeAttack = TimeAttackRate;
            }
        }
        else animator.SetBool("IsIdiel", true);
    }

    void FlameAttack()
    {
        if (!Player) return;
        float d = Vector3.Distance(transform.position, Player.position);
        if (d <= fireBallRange)
        {
            fireBallTimer -= Time.deltaTime;
            if (fireBallTimer <= 0f)
            {
                animator.SetTrigger("IsFlameAttack");
                var fireBall = Instantiate(fireBallPrefab, firePoint.position, Quaternion.identity);

                Vector2 dir = (Player.position - firePoint.position).normalized;
                fireBall.GetComponent<Rigidbody2D>().linearVelocity = dir * fireBallSpeed;
                fireBall.transform.localScale = (dir.x < 0) ? new Vector3(-1,1,1) : new Vector3(1,1,1);
                fireBallTimer = fireBallCooldown;
            }
        }
        else animator.SetBool("IsIdiel2", true);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if (isInvulnerable) { Destroy(collision.gameObject); return; }

            currentHPEnemy -= 1000;
            if (currentHPEnemy < 0) currentHPEnemy = 0;
            swordEffect.Play();
            UpdateHP();

            if (currentHPEnemy <= 0 && !isDead) StartCoroutine(DeadEffect());
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator DeadEffect()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsRunning", false);

        deadEffect.Play(); bloodEffect.Play();
        healthSlider.gameObject.SetActive(false);
        hpBossText.gameObject.SetActive(false);
        if (dragonBossAudio.isPlaying) dragonBossAudio.Stop();

        yield return new WaitForSeconds(1f);
        portalEnd.SetActive(true);
        tuong.SetActive(false);
        Destroy(gameObject);
    }

    // ===================== AUDIO & CUTSCENE DIRECTOR =====================
    private IEnumerator AudioDirector()
    {
        // Phase1 intro
        PlayClip(BossPhase1, loop: false);
        yield return new WaitForSeconds(GetClipLenSafe(BossPhase1));

        // Phase1 loop
        PlayClip(BossPhase1Loop, loop: true);

        // Đợi tới 50%
        while (HPPercent() > 0.5f && !isDead) yield return null;

        // MID TRANSITION: bất tử, khoá player, cam Boss, Panel + typewriter 2 đoạn (mỗi đoạn 5s)
        if (!midPhaseStarted && !isDead)
        {
            phase = BossPhase.MidTransition;
            midPhaseStarted = true;

            // Bật mid ngay lập tức (cắt loop)
            PlayClip(BossMidPhase1, loop: false);

            // Vào mid: set bất tử + khoá player + focus cam Boss
            EnterMidPhase();

            // Hiện panel & chạy 2 đoạn thoại
            yield return StartCoroutine(ShowMidPhaseDialog());

            // Kết thúc mid: tắt panel, mở player, teleport + chém, vào Phase2 + nhạc loop
            ExitMidPhase();
            TeleportNextToPlayerAndSlash();

            if (!lastPhaseStarted && !isDead)
            {
                phase = BossPhase.Phase2;
                lastPhaseStarted = true;
                PlayClip(BossLastPhase, loop: true);
                FocusBossCam(false); // trả camera về Player sau cutscene
            }
        }
    }

    private void PlayClip(AudioClip clip, bool loop)
    {
        if (!dragonBossAudio) return;
        dragonBossAudio.Stop();
        dragonBossAudio.clip = clip;
        dragonBossAudio.loop = loop;
        if (clip) dragonBossAudio.Play();
    }

    private float GetClipLenSafe(AudioClip clip) => (clip ? clip.length : 0f);
    private float HPPercent() => (maxHP <= 0) ? 0f : Mathf.Clamp01((float)currentHPEnemy / maxHP);

    private void EnterMidPhase()
    {
        isInvulnerable = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsRunning", false);

        // Khoá player điều khiển
        var pComp = Player ? Player.GetComponent<Player>() : null;
        if (pComp) { pComp.canMove = false; } // Player script của bạn có sẵn biến này

        // Camera về Boss
        FocusBossCam(true);
    }

    private void ExitMidPhase()
    {
        isInvulnerable = false;

        // Mở khoá player
        var pComp = Player ? Player.GetComponent<Player>() : null;
        if (pComp) { pComp.canMove = true; }
    }

    // ---------- Mid-phase dialog (typewriter 2 đoạn) ----------
    private IEnumerator ShowMidPhaseDialog()
    {
        if (!panelBossMessage || !bossMessageTMP)
            yield break;

        panelBossMessage.SetActive(true);

        string line1 = "Người cũng khá lắm, người khiến ta phải rơi vào tình trạng lúc này là người cũng gọi là có thực lực, nhưng mà...";
        string line2 = "Nhưng mà, nhiêu đó vẫn chưa đủ đâu, giờ, ta sẽ bắt đầu nghiêm túc với người, người hãy chuẩn bị xuống địa ngục đi nào!";

        // chạy từng chữ trong đúng 5s/đoạn
        yield return StartCoroutine(TypeText(bossMessageTMP, line1, 5f));
        yield return StartCoroutine(TypeText(bossMessageTMP, line2, 5f));

        panelBossMessage.SetActive(false);
    }

    private IEnumerator TypeText(TextMeshProUGUI tmp, string full, float duration)
    {
        tmp.text = "";
        if (string.IsNullOrEmpty(full) || duration <= 0f)
        {
            tmp.text = full;
            yield return null;
            yield break;
        }

        int len = full.Length;
        float interval = duration / Mathf.Max(1, len);

        for (int i = 1; i <= len; i++)
        {
            tmp.text = full.Substring(0, i);
            yield return new WaitForSeconds(interval);
        }
    }

    // ---------- Teleport helpers ----------
    private void TeleportNextToPlayerAndSlash()
    {
        if (!Player) return;

        Vector3 offset = new Vector3(right ? 1.0f : -1.0f, 0f, 0f);
        transform.position = Player.position + offset;

        bool faceLeft = (Player.position.x < transform.position.x);
        if ((faceLeft && right) || (!faceLeft && !right))
        {
            right = !right;
            var sc = transform.localScale; sc.x *= -1; transform.localScale = sc;
        }

        animator.SetTrigger("IsNormalAttack");
        var oneSkill = Instantiate(hitbox, Knifedamage.position, Quaternion.identity);
        Destroy(oneSkill, 0.1f);
        timeAttack = TimeAttackRate;
    }

    private void TeleportCloseToPlayer()
    {
        if (!Player) return;

        Vector3 offset = new Vector3(right ? 1.0f : -1.0f, 0f, 0f);
        transform.position = Player.position + offset;

        bool faceLeft = (Player.position.x < transform.position.x);
        if ((faceLeft && right) || (!faceLeft && !right))
        {
            right = !right;
            var sc = transform.localScale; sc.x *= -1; transform.localScale = sc;
        }
    }
}
