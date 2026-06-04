using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject menuPanel;

    public bool gameStarted = false;


    void Start()
    {
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        menuPanel.SetActive(false);
        gameStarted = true;
        Time.timeScale = 1f;
    }
}