using System.Collections;
using UnityEngine;

/// <summary>
/// Sahurnita Boss AI - 2D Pixel Boss Fighter
/// Requires: Rigidbody2D, Collider2D, Animator (optional)
/// 
/// PHASES:
///   Phase 1 (100% - 50% HP): Patrol + Charge + Slam
///   Phase 2 (below 50% HP):  Faster + adds Projectile Burst
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SahurnitaBoss : MonoBehaviour
{
    // ───────────────────────────── Inspector ─────────────────────────────

    [Header("Stats")]
    public float maxHealth = 300f;

    [Header("Movement")]
    public float patrolSpeed    = 2.5f;
    public float patrolDistance = 4f;      // units left/right from spawn
    public float phase2SpeedMult = 1.5f;

    [Header("Charge Attack")]
    public float chargeSpeed       = 12f;
    public float chargeWindupTime  = 0.6f;  // pause before charging
    public float chargeCooldown    = 3.5f;

    [Header("Slam Attack")]
    public float slamCooldown      = 5f;
    public float slamRadius        = 2.5f;
    public float slamDamage        = 25f;
    public LayerMask playerLayer;

    [Header("Projectile Burst (Phase 2)")]
    public GameObject projectilePrefab;     // assign a prefab in Inspector
    public float projectileSpeed   = 8f;
    public int   burstCount        = 5;
    public float burstCooldown     = 4f;

    [Header("Damage")]
    public float chargeDamage      = 20f;
    public float contactDamage     = 10f;

    [Header("References")]
    public Transform player;               // drag player Transform here
    public Animator  anim;                 // optional animator

    // ─────────────────────────── Private State ───────────────────────────

    private Rigidbody2D rb;
    private float       currentHealth;
    private bool        isPhase2;

    // Timers
    private float chargeTimer;
    private float slamTimer;
    private float burstTimer;

    // Patrol
    private Vector2 patrolOrigin;
    private int     patrolDir = 1;

    // State machine
    private enum BossState { Patrolling, Windup, Charging, Slamming, BurstAttack, Dead }
    private BossState state = BossState.Patrolling;

    private bool isActing;   // blocks new actions while one is running

    // ─────────────────────────────── Init ────────────────────────────────

    void Start()
    {
        rb            = GetComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.constraints  = RigidbodyConstraints2D.FreezeRotation;

        currentHealth = maxHealth;
        patrolOrigin  = transform.position;

        // Auto-find player if not assigned
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    // ──────────────────────────── Main Loop ──────────────────────────────

    void Update()
    {
        if (state == BossState.Dead) return;

        TickTimers();
        CheckPhaseTransition();

        if (!isActing)
            DecideAction();
    }

    void FixedUpdate()
    {
        if (state == BossState.Patrolling)
            DoPatrol();
    }

    // ────────────────────────── Phase Transition ─────────────────────────

    void CheckPhaseTransition()
    {
        if (!isPhase2 && currentHealth <= maxHealth * 0.5f)
        {
            isPhase2 = true;
            patrolSpeed  *= phase2SpeedMult;
            chargeSpeed  *= phase2SpeedMult;
            chargeCooldown = Mathf.Max(1.5f, chargeCooldown - 1f);
            Debug.Log("Sahurnita enters Phase 2!");
            SetAnim("Phase2", true);
        }
    }

    // ─────────────────────────── Decision AI ─────────────────────────────

    void TickTimers()
    {
        chargeTimer += Time.deltaTime;
        slamTimer   += Time.deltaTime;
        if (isPhase2) burstTimer += Time.deltaTime;
    }

    void DecideAction()
    {
        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        // Priority order:
        // 1. Slam  – player is close
        // 2. Burst – phase 2 cooldown ready
        // 3. Charge – player is further away
        // (otherwise patrol)

        if (slamTimer >= slamCooldown && distToPlayer <= slamRadius)
        {
            slamTimer = 0f;
            StartCoroutine(SlamAttack());
        }
        else if (isPhase2 && burstTimer >= burstCooldown)
        {
            burstTimer = 0f;
            StartCoroutine(ProjectileBurst());
        }
        else if (chargeTimer >= chargeCooldown && distToPlayer > slamRadius)
        {
            chargeTimer = 0f;
            StartCoroutine(ChargeAttack());
        }
        else
        {
            state = BossState.Patrolling;
        }
    }

    // ──────────────────────────── Patrol ────────────────────────────────

    void DoPatrol()
    {
        float target = patrolOrigin.x + patrolDir * patrolDistance;
        float move   = patrolDir * patrolSpeed;

        rb.linearVelocity = new Vector2(move, rb.linearVelocity.y);

        // Flip at patrol edges
        if (Mathf.Abs(transform.position.x - patrolOrigin.x) >= patrolDistance)
            patrolDir *= -1;

        FaceDirection(patrolDir);
        SetAnim("IsWalking", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
    }

    // ─────────────────────────── Charge Attack ───────────────────────────

    IEnumerator ChargeAttack()
    {
        isActing = true;
        state    = BossState.Windup;

        if (player == null) { isActing = false; yield break; }

        // Face the player
        int dir = (player.position.x > transform.position.x) ? 1 : -1;
        FaceDirection(dir);

        SetAnim("Windup", true);
        yield return new WaitForSeconds(chargeWindupTime);
        SetAnim("Windup", false);

        // Charge
        state = BossState.Charging;
        SetAnim("IsCharging", true);

        float chargeTime = 0.4f;
        float elapsed    = 0f;

        while (elapsed < chargeTime)
        {
            rb.linearVelocity = new Vector2(dir * chargeSpeed, rb.linearVelocity.y);
            elapsed += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        SetAnim("IsCharging", false);

        state    = BossState.Patrolling;
        isActing = false;
    }

    // ──────────────────────────── Slam Attack ────────────────────────────

    IEnumerator SlamAttack()
    {
        isActing = true;
        state    = BossState.Slamming;

        SetAnim("IsSlam", true);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        yield return new WaitForSeconds(0.3f);   // slam windup

        // Shockwave damage
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, slamRadius, playerLayer);
        foreach (var hit in hits)
        {
            var hp = hit.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(slamDamage);
        }

        Debug.Log("Sahurnita SLAMS!");
        yield return new WaitForSeconds(0.4f);   // recovery

        SetAnim("IsSlam", false);
        state    = BossState.Patrolling;
        isActing = false;
    }

    // ──────────────────────── Projectile Burst (P2) ──────────────────────

    IEnumerator ProjectileBurst()
    {
        isActing = true;
        state    = BossState.BurstAttack;

        SetAnim("IsBurst", true);
        rb.linearVelocity = Vector2.zero;

        if (projectilePrefab != null)
        {
            float angleStep = 360f / burstCount;
            for (int i = 0; i < burstCount; i++)
            {
                float   angle = i * angleStep;
                Vector2 dir   = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad));

                GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
                if (projRb != null) projRb.linearVelocity = dir * projectileSpeed;

                yield return new WaitForSeconds(0.08f);
            }
        }
        else
        {
            Debug.LogWarning("Sahurnita: No projectile prefab assigned!");
        }

        yield return new WaitForSeconds(0.5f);

        SetAnim("IsBurst", false);
        state    = BossState.Patrolling;
        isActing = false;
    }

    // ──────────────────────────── Take Damage ────────────────────────────

    public void TakeDamage(float amount)
    {
        if (state == BossState.Dead) return;

        currentHealth -= amount;
        SetAnim("Hit", true);
        StartCoroutine(ResetHitAnim());

        Debug.Log($"Sahurnita HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    IEnumerator ResetHitAnim()
    {
        yield return new WaitForSeconds(0.1f);
        SetAnim("Hit", false);
    }

    // ────────────────────────────── Death ────────────────────────────────

    void Die()
    {
        state         = BossState.Dead;
        rb.linearVelocity   = Vector2.zero;
        isActing      = false;

        SetAnim("IsDead", true);
        Debug.Log("Sahurnita has been defeated!");

        // Disable collision so player doesn't interact with corpse
        GetComponent<Collider2D>().enabled = false;

        // Destroy after death animation plays (adjust delay as needed)
        Destroy(gameObject, 2f);
    }

    // ─────────────────────────── Collision ───────────────────────────────

    void OnCollisionEnter2D(Collision2D col)
    {
        if (state == BossState.Dead) return;

        if (col.gameObject.CompareTag("Player"))
        {
            float dmg = (state == BossState.Charging) ? chargeDamage : contactDamage;
            var hp = col.gameObject.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(dmg);
        }
    }

    // ──────────────────────────── Helpers ────────────────────────────────

    void FaceDirection(int dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    void SetAnim(string param, bool value)
    {
        if (anim != null) anim.SetBool(param, value);
    }

    // Gizmos – visualise slam radius in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slamRadius);

        Gizmos.color = Color.yellow;
        Vector3 origin = Application.isPlaying ? (Vector3)patrolOrigin : transform.position;
        Gizmos.DrawLine(origin + Vector3.left * patrolDistance, origin + Vector3.right * patrolDistance);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// Stub – replace with your own PlayerHealth component if you have one
// ═══════════════════════════════════════════════════════════════════════════
public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Player HP: {health}");
        if (health <= 0f) Debug.Log("Player died!");
    }
}
