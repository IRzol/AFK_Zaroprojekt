using UnityEngine;
using UnityEngine.Tilemaps;

public class OpenDoor : MonoBehaviour
{
    public GameObject doorHitb;
    public Animator animator;
    public float interactRange = 2f;
    public Transform player;
    public GameObject eButton; 
    private bool opened = false;
    public Transform interactPoint;

    void Start()
    {
        eButton.SetActive(true);
    }

    void Update()
    {
        if (opened)
        {
            eButton.SetActive(false);
            return;
        }

        float distance1 = Vector2.Distance(interactPoint.position, player.position);
        Debug.Log("Player: " + player.position);
        Debug.Log("Interact: " + interactPoint.position);
        Debug.Log("Distance: " + distance1);
        Debug.Log(gameObject.name + " -> " + interactPoint.name);
        bool inRange = distance1 <= interactRange;

        eButton.SetActive(inRange);

        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            doorHitb.GetComponent<TilemapCollider2D>().enabled = false;
            animator.SetBool("IsOpen", true);
            opened = true;
        }
    }


}