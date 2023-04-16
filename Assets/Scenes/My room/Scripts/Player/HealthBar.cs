using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public Slider healthBar;
    public Image fill;
    public Gradient healthBarGradient;

    void Start()
    {
        playerHealth = PlayerHealth.Instance;
        SetMaxHealth();
    }

    void Update()
    {
        SetHealth();
    }

    public void SetMaxHealth()
    {
        healthBar.maxValue = playerHealth.maxHealth;
        healthBar.value = playerHealth.maxHealth;
        fill.color = healthBarGradient.Evaluate(1f);
    }

    public void SetHealth()
    {
        healthBar.value = playerHealth.currentHealth;
        fill.color = healthBarGradient.Evaluate(healthBar.normalizedValue);
    }
}
