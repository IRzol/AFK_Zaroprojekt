using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    public float health = 100f;

    void Awake()
    {
        Instance = this;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        Debug.Log($"Player HP: {health}");
    }
}