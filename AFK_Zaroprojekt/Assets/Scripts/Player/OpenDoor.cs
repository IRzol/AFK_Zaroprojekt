using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public GameObject doorHitb;
    public float interactRange = 2f;
    public Transform player;

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            doorHitb.GetComponent<TilemapCollider2D>().enabled = false;
        }
    }
}