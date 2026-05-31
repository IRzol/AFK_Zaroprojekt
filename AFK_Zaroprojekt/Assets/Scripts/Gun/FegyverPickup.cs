using UnityEngine;

public class FegyverPickup : MonoBehaviour
{
    public GameObject fegyverPrefab;
    public GameObject bulletPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerFegyverController ctrl = other.GetComponent<PlayerFegyverController>();
            if (ctrl != null)
            {
                ctrl.FegyverFelvet(fegyverPrefab);
                Destroy(gameObject);
            }
        }
    }
}