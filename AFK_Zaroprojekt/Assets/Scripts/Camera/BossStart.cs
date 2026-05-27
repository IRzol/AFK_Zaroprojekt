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
    public GameObject eButton2;
    public GameObject skull;
    public GameObject doorHitbox;
    public Animator doorAnimator;
    public Animator skullAnimator;
    void Update()
    {
        if (triggered)
        {
            eButton2.SetActive(false);
            return;
        }

        float distance2 = Vector2.Distance(transform.position, player.position);
        bool inRange2 = distance2 <= interactRange2;

        eButton2.SetActive(inRange2);

        if (inRange2 && Input.GetKeyDown(KeyCode.E))
        {
            doorHitbox.GetComponent<TilemapCollider2D>().enabled = true;
            doorAnimator.SetBool("IsOpen", false);
            skullAnimator.SetBool("isPressed", true);
            triggered = true;
            followCam.Priority = 0;
            staticCamBossArena.Priority = 10;
        }
    }
    
}