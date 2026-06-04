using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameManager gameManager;


    void Update()
    {
        if (gameManager == null || settingsPanel == null)
        {
            Debug.Log("UIManager: gameManager vagy settingsPanel null!");
            return;
        }

        if (!gameManager.gameStarted)
        {
            Debug.Log("UIManager: gameStarted = false, nem fut tovabb");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("UIManager: Escape lenyomva!");
            bool isOpen = !settingsPanel.activeSelf;
            settingsPanel.SetActive(isOpen);
            Time.timeScale = isOpen ? 0f : 1f;
        }
    }
}