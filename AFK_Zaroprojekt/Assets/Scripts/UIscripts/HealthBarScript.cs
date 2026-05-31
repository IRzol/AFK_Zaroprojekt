using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthBarSlider;
    public TextMeshProUGUI healthBarValueText;

    public float maxHealth;
    public float currHealth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        healthBarValueText.text = currHealth.ToString() + "/" + maxHealth.ToString();
        healthBarSlider.value = currHealth;
        healthBarSlider.maxValue = maxHealth;
    }
}
