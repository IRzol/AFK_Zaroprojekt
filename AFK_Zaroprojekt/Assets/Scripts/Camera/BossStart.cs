using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossStart : MonoBehaviour
{
    public CinemachineCamera staticCamBossArena;
    public CinemachineCamera followCam;
    private bool triggered = false;
    public float interactRange = 2f;
    public Transform player;
    public GameObject eButton;
    public GameObject skull;
    public GameObject doorHitbox;
    public Animator animator;
    void Update()
    {
        if (triggered)
        {
            eButton.SetActive(false);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        bool inRange = distance <= interactRange;

        eButton.SetActive(inRange);

        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            followCam.Priority = 0;
            staticCamBossArena.Priority = 10;
            triggered = true;
            skull.GetComponent<Renderer>().enabled = false;
            doorHitbox.GetComponent<TilemapCollider2D>().enabled = true;
            animator.SetBool("IsOpen", false);
        }
    }
    
}