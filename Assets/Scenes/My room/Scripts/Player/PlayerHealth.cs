using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth;

    public GameObject deathPanel;

    private static PlayerHealth instance;
    public static PlayerHealth Instance
    {
        get
        {
            if(instance == null) instance = GameObject.FindObjectOfType<PlayerHealth>();
            return instance;
        }
    }

    void Start()
    {
        deathPanel.SetActive(false);
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if(currentHealth <= 0)
            Die();
    }

    void Die()
    {
        currentHealth = 0;
        MyPlayer.Instance.IsFrozen = true;
        MyPlayer.Instance.dropAction.Disable();
        MyPlayer.Instance.throwAction.Disable();
        MyPlayer.Instance.jumpAction.Disable();
        MyPlayer.Instance.interactAction.Disable();
        Time.timeScale = 0f;
        deathPanel.SetActive(true);
    }



}
