using System.Collections;
using UnityEngine;

public class FegyverSpawner : MonoBehaviour
{
    [Header("Beállítások")]
    public GameObject[] fegyverPrefabok;
    public float minSpawnIdo = 5f;
    public float maxSpawnIdo = 15f;

    private GameObject jelenlegiFegyver;

    void Start()
    {
        StartCoroutine(SpawnRutin());
    }

    IEnumerator SpawnRutin()
    {
        while (true)
        {
            float randomVarakozas = Random.Range(minSpawnIdo, maxSpawnIdo);
            yield return new WaitForSeconds(randomVarakozas);

            if (jelenlegiFegyver == null)
            {
                SpawnFegyver();
            }
        }
    }

    void SpawnFegyver()
    {
        if (fegyverPrefabok.Length == 0)
        {
            Debug.LogWarning("Nincs fegyver berakva a Spawner listájába!");
            return;
        }

        int randomIndex = Random.Range(0, fegyverPrefabok.Length);
        jelenlegiFegyver = Instantiate(fegyverPrefabok[randomIndex], transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}