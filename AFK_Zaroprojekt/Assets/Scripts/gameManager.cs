using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject healthBar;
    public GameObject staminaBar;
    public GameObject weaponIcon;
    public bool gameStarted = false;


    void Start()
    {
        healthBar.SetActive(false);
        staminaBar.SetActive(false);
        weaponIcon.SetActive(false);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        healthBar.SetActive(true);
        staminaBar.SetActive(true);
        weaponIcon.SetActive(true);
        gameStarted = true;
        Time.timeScale = 1f;
    }
}