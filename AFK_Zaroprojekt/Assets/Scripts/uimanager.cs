using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameManager gameManager;


    void Update()
    {
        if (gameManager == null || settingsPanel == null)
            return;

        if (!gameManager.gameStarted)
            return;

        // Game over alatt ne nyíljon meg a settings
        if (gameManager.isGameOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isOpen = !settingsPanel.activeSelf;
            settingsPanel.SetActive(isOpen);
            Time.timeScale = isOpen ? 0f : 1f;
        }
    }
}