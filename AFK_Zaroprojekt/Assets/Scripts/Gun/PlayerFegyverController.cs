using UnityEngine;

public class PlayerFegyverController : MonoBehaviour
{
    public Transform fegyverTartoPont;
    private GameObject felvetFegyver;

    public void FegyverFelvet(GameObject fegyverPrefab)
    {
        if (felvetFegyver != null) Destroy(felvetFegyver);

        felvetFegyver = Instantiate(fegyverPrefab,
                        fegyverTartoPont.position,
                        fegyverTartoPont.rotation);
        felvetFegyver.transform.SetParent(fegyverTartoPont);

        Rigidbody2D rb = felvetFegyver.GetComponent<Rigidbody2D>();
        if (rb != null) rb.bodyType = RigidbodyType2D.Kinematic;

        Collider2D col = felvetFegyver.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        GetComponent<Shooting>().hasGun = true;

        Transform weaponFirePoint = felvetFegyver.transform.Find("FirePoint");
        if (weaponFirePoint != null)
        {
            GetComponent<Shooting>().firePoint = weaponFirePoint;
        }
        FegyverPickup fp = fegyverPrefab.GetComponent<FegyverPickup>();
        if (fp != null && fp.bulletPrefab != null)
            GetComponent<Shooting>().bulletPrefab = fp.bulletPrefab;
    }

}
