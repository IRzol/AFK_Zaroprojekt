using Unity.Cinemachine;
using UnityEngine;

public class CameraFollowTrigger : MonoBehaviour
{
    public CinemachineCamera staticCam;
    public CinemachineCamera followCam;
    private bool triggered = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (other.CompareTag("Player"))
        {
            staticCam.Priority = 0;  // static loses control
            followCam.Priority = 10; // follow takes over
            triggered = true;
        }
    }
}