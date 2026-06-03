using System.Collections;
using UnityEngine;

public class Sahur : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 200f;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    [Header("References")]
    public Transform player;

    private float currentHealth;
    private float attackTimer;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        attackTimer += Time.deltaTime;

        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (distToPlayer <= attackRange)
        {
            // Támadótávolságon belül - üt
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0f;
                Attack();
            }
        }
        else
        {
            // Player felé sétál
            int dir = (player.position.x > transform.position.x) ? 1 : -1;
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            FaceDirection(dir);
        }
    }

    void Attack()
    {
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
            Debug.Log("Sahur hit the player!");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        Debug.Log($"Sahur HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0f) Die();
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Sahur defeated!");
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }

    void FaceDirection(int dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * dir;
        transform.localScale = scale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}