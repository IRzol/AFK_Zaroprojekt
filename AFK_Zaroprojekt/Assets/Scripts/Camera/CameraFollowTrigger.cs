using UnityEngine;
using Unity.Cinemachine;

public class CameraFollowTrigger : MonoBehaviour
{
    public CinemachineCamera staticCamBossArena;
    public CinemachineCamera followCam;
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            if (staticCamBossArena != null) staticCamBossArena.Priority = 0;
            if (followCam != null) followCam.Priority = 10;
            triggered = true;
        }
    }
}