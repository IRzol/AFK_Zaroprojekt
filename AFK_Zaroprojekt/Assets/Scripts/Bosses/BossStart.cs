using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossStart : MonoBehaviour
{
    public CinemachineCamera staticCamBossArena;
    public CinemachineCamera followCam;
    private bool triggered = false;
    public float interactRange2 = 2f;
    public Transform player;
    public GameObject eButtonSkull;
    public GameObject lilskull;
    public GameObject doorHitbox;
    public Animator doorAnimator;
    public Animator skullAnimator;
    public GameObject skullBoss;
    private void Start()
    {
        skullBoss.GetComponent<SpriteRenderer>().enabled = false;
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
            doorHitbox.GetComponent<TilemapCollider2D>().enabled = true;
            doorAnimator.SetBool("IsOpen", false);
            skullAnimator.SetBool("isPressed", true);
            triggered = true;
            followCam.Priority = 0;
            staticCamBossArena.Priority = 10;
            skullBoss.GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    
}