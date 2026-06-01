using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Unity.Cinemachine;

public class SkullBossHealth : MonoBehaviour
{
    public float health = 600f;
    public float maxHealth = 600f;

    public Image healthBar;
    public Image damageBar;
    public TMP_Text healthText;
    private float barTimer;
    public GameObject bossHealthUI;
    private SpriteRenderer spriteRenderer;

    public GameObject doorHitbox;
    public Animator doorAnimator;
    public CinemachineCamera staticCamBossArena;
    public CinemachineCamera followCam;
    public Animator skullAnimator;
    private bool dead = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        bossHealthUI.SetActive(false);

        healthBar.fillAmount = 1f;
        damageBar.fillAmount = 1f;
        
    }

    void Update()
    {
        if (dead) return;

        if (healthBar != null && damageBar != null)
        {
            //Azonnali HP bar
            healthBar.fillAmount = health / maxHealth;

            //Várakozás sebzõdés után
            if (barTimer > 0)
            {
                barTimer -= Time.deltaTime;
            }
            else
            {
                //Késleltetett HP bar
                damageBar.fillAmount = Mathf.Lerp(damageBar.fillAmount, healthBar.fillAmount, 2f * Time.deltaTime);
            }
        }

       
        healthText.text = Mathf.RoundToInt(health) + "/" + Mathf.RoundToInt(maxHealth);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dead) return;

        if (other.CompareTag("DamageToBoss"))
        {   
            health -= 16.6667f;
            health = Mathf.Clamp(health, 0, maxHealth);
            barTimer = 0.5f;
            StartCoroutine(BlinkRed());
            //if (health <= 0)
            //{
            //    Die();
            //}
        }
    }

    private IEnumerator BlinkRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    private IEnumerator Die()
    {
        SkullBossAttack attack = GetComponent<SkullBossAttack>();
        if (attack != null)
        {
            attack.isAlive = false;
            attack.StopAllCoroutines();
            skullAnimator.Play("SkullBoss_Burn");
            yield return new WaitForSeconds(0.5f);
            attack.enabled = false;
            yield return new WaitForSeconds(1.5f);
        }
        followCam.Priority = 10;
        staticCamBossArena.Priority = 0;
        bossHealthUI.SetActive(false);
        doorHitbox.GetComponent<TilemapCollider2D>().enabled = false;
        doorAnimator.SetBool("IsOpen", true);
        Destroy(gameObject);
    }

}