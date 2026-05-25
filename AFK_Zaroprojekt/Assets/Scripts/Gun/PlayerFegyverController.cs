using UnityEngine;

public class PlayerFegyverController : MonoBehaviour
{
    public Transform fegyverTartoPont;
    private GameObject felvetFegyver;

    public void FegyverFelvet(GameObject fegyverPrefab)
    {
        if (felvetFegyver != null) Destroy(felvetFegyver);

        felvetFegyver = Instantiate(fegyverPrefab, fegyverTartoPont.position, fegyverTartoPont.rotation);
        felvetFegyver.transform.SetParent(fegyverTartoPont);
    }
}