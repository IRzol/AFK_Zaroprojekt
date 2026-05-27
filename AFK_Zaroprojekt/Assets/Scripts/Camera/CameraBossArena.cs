using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraBossArena : MonoBehaviour
{
    public CinemachineCamera staticCamBossArena;
    public CinemachineCamera followCam;
    private bool triggered = false;
    public float interactRange = 2f;
    public Transform player;
    public GameObject eButton;

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
        }
    }
    
}