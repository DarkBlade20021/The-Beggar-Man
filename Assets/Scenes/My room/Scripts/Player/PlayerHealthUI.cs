using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    private PlayerHealth playerHealth;
    public TMP_Text healthText;

    void Start()
    {
        playerHealth = PlayerHealth.Instance;
    }

    void Update()
    {
        healthText.text = playerHealth.currentHealth + " / " + playerHealth.maxHealth;
    }
}
