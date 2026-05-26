using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public GameObject doorHitb;
    public Animator animator;
    public float interactRange = 2f;
    public Transform player;
    private bool opened = false;

    void Update()
    {
        if (opened) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            doorHitb.GetComponent<TilemapCollider2D>().enabled = false;
            animator.SetBool("IsOpen", true); //
            opened = true;
        }
    }
}