using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkullBossFight : MonoBehaviour
{
    public CinemachineCamera staticCamBossArena;
    public CinemachineCamera followCam;
    private bool triggered = false;
    public float interactRange2 = 1.5f;
    public Transform player;
    public GameObject eButtonSkull;
    public GameObject lilskull;
    public GameObject doorHitbox;
    public Animator doorAnimator;
    public Animator lilSkullAnimator;
    public Animator bossSkullAnimator;
    public GameObject skullBoss;
    public GameObject sahurBoss;
    public Sprite SkullBoss_0;

    private void Start()
    {
        skullBoss.SetActive(false);
        sahurBoss.GetComponent<SpriteRenderer>().enabled = false;

    }
    void Update()
    {
        if (triggered)
        {
            eButtonSkull.SetActive(false);

            return;
        }

        float distance2 = Vector2.Distance(transform.position, player.position);
        bool inRange2 = distance2 <= interactRange2;

        eButtonSkull.SetActive(inRange2);

        if (inRange2 && Input.GetKeyDown(KeyCode.E))
        {
            BossStart();
        }
    }

    private void BossStart()
    {
        doorHitbox.GetComponent<TilemapCollider2D>().enabled = true;
        doorAnimator.SetBool("IsOpen", false);
        lilSkullAnimator.SetBool("isPressed", true);
        triggered = true;
        followCam.Priority = 0;
        staticCamBossArena.Priority = 10;
        StartCoroutine(BossStartAnimacio());
        sahurBoss.GetComponent<SpriteRenderer>().enabled = true;
    }

    private IEnumerator BossStartAnimacio()
    {
        skullBoss.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        skullBoss.GetComponent<SpriteRenderer>().enabled = true;
        bossSkullAnimator.SetBool("isSpawning", true);
        yield return new WaitForSeconds(1f);
        bossSkullAnimator.SetBool("isLaughing", true);
        yield return new WaitForSeconds(3f);
        bossSkullAnimator.SetBool("isLaughing", false);
        bossSkullAnimator.SetBool("isSpawning", false);
        skullBoss.GetComponent<SkullBossAttack>().enabled = true;
    }
}