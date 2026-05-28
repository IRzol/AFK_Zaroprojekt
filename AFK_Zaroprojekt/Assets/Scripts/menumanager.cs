using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool isOpen = !settingsPanel.activeSelf;
            settingsPanel.SetActive(isOpen);

            Time.timeScale = isOpen ? 0f : 1f;
        }
    }

}
