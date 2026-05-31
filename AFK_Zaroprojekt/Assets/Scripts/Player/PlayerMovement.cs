using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float health = 100f;
    public float stamina = 100f;
    private float regenTimer = 0f;

    public Transform fegyverTartoPont;
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 20f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.7f;


    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public bool isGrounded;
    [SerializeField] private TrailRenderer tr;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    public float maxHealth;
    public Image healthBar;
    public float maxStamina;
    public Image staminaBar;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        maxHealth = health;
        maxStamina = stamina;
    }

    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");
        if (!isDashing)
        {
            if (stamina <= 0)
            {
                moveInput = moveInput / 2;
                jumpForce = 7f;
                dashingPower = 7f;
            }

            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }

            SetAnimation(moveInput);

            if (moveInput > 0)
            {
                sr.flipX = false;
                fegyverTartoPont.localScale = new Vector3(1, 1, 1);
            }
            else if (moveInput < 0)
            {
                sr.flipX = true;
                fegyverTartoPont.localScale = new Vector3(-1, 1, 1);
            }
        }

        Stamina(moveInput);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        }
    }

    private void Stamina(float moveInput)
    {
        if (staminaBar != null)
        {
            if (stamina >= 0)
            {
                if (isDashing)
                {
                    stamina -= 25f * Time.deltaTime;
                    regenTimer = 0f;
                }
                if (moveInput != 0)
                {
                    stamina -= 12f * Time.deltaTime;
                    regenTimer = 0f;
                }
                
            }
            if (moveInput < 0.1 && moveInput > -0.1)
            {
                regenTimer += 1 * Time.deltaTime;
            }
            if (regenTimer >= 1.2f)
            {
                stamina += 20f * Time.deltaTime; // regen
            }


            staminaBar.fillAmount = Mathf.Clamp(stamina / maxStamina, 0, 1);
        }
    }
    

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void SetAnimation(float moveInput)
    {
        if (isGrounded) 
        {
            if(moveInput == 0)
            {
                animator.Play("Player_Idle");
            }
            else
            {
                animator.Play("Player_Run");
            }
        }
        else
        {
            if(rb.linearVelocityY > 0)
            {
                animator.Play("Player_Jump");
            }
            else
            {
                animator.Play("Player_Fall");
            }
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float direction = sr.flipX ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        rb.linearVelocity = Vector2.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Damage"))
        {
            health -= 16.666666666666666666666666666667f;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce-2f);
            StartCoroutine(BlinkRed());
            if (health <= 2f)
            {
                Die();
            }
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }
}
