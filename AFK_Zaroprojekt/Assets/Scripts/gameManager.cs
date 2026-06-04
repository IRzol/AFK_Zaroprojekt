using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject healthBar;
    public GameObject staminaBar;
    public bool gameStarted = false;


    void Start()
    {
        healthBar.SetActive(false);
        staminaBar.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        healthBar.SetActive(true);
        staminaBar.SetActive(true);
        gameStarted = true;
        Time.timeScale = 1f;
    }
}