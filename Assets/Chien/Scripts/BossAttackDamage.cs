using UnityEngine;

public class BossAttackDamage : MonoBehaviour
{
    public float damageAttack1 = 50f;
    public float damageAttack2 = 100f;
    public float attackCooldown = 1f; // thời gian giữa 2 lần gây damage

    private float lastAttackTime1 = -Mathf.Infinity;
    private float lastAttackTime2 = -Mathf.Infinity;

    private Animator animator;
    private Player player;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
        if (foundPlayer != null)
            player = foundPlayer.GetComponent<Player>();
    }

    private void Update()
    {
        if (player == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Tấn công 1
        if (stateInfo.IsName("Wooden Aarakocra Attack 1Animation") && Time.time - lastAttackTime1 > attackCooldown)
        {
            player.TakeDamage(damageAttack1);
            lastAttackTime1 = Time.time;
        }

        // Tấn công 2
        if (stateInfo.IsName("Wooden Aarakocra Attack 2 Animation") && Time.time - lastAttackTime2 > attackCooldown)
        {
            player.TakeDamage(damageAttack2);
            lastAttackTime2 = Time.time;
        }
    }
}
