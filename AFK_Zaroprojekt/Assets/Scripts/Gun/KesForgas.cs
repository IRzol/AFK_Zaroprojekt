using UnityEngine;
public class KesForgas : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0, 0, 720 * Time.deltaTime);
    }
}