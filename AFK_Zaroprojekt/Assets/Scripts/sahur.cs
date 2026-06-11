using UnityEngine;
using TMPro;
using System.Collections;

public class Sahur : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 200f;
    public float moveSpeed = 2f;
    public float attackDamage = 10f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    [Header("References")]
    public TMP_Text healthText;
    public GameObject sahurHP;

    private float currentHealth;
    private float attackTimer;
    private Rigidbody2D rb;
    private bool isDead = false;
    private Transform player;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogError("PLAYER NEM TALALHATO! Ellenorizd a Player taget!");
    }

    void Update()
    {
        if (isDead || player == null) return;

        attackTimer += Time.deltaTime;
        float dist = Vector2.Distance(transform.position, player.position);

        // HP UI
        if (sahurHP != null)
            sahurHP.SetActive(dist <= 8f);
        if (healthText != null)
            healthText.text = Mathf.RoundToInt(currentHealth) + "/" + Mathf.RoundToInt(maxHealth);

        if (dist <= attackRange)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            if (attackTimer >= attackCooldown)
            {
                attackTimer = 0f;
                Attack();
            }
        }
        else
        {
            int dir = (player.position.x > transform.position.x) ? 1 : -1;
            rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);
            FaceDirection(dir);
        }
    }

    void Attack()
    {
        Debug.Log("Attack() MEGHIVVA!");

        if (PlayerHealth.Instance == null)
        {
            Debug.LogError("PlayerHealth.Instance NULL! Tedd ra a PlayerHealth scriptet a Playerre!");
            return;
        }

        PlayerHealth.Instance.TakeDamage(attackDamage);
        Debug.Log("Sebzes: " + attackDamage + " | Player HP: " + PlayerHealth.Instance.health);
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;
        currentHealth -= amount;
        StartCoroutine(BlinkRed());
        if (currentHealth <= 0f) Die();
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }

    void FaceDirection(int dir)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * -dir;
        transform.localScale = scale;
    }

    private IEnumerator BlinkRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DamageToBoss"))
            TakeDamage(16.6667f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}