using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerStamina : MonoBehaviour
{
    [Header("Properties")]
    public float stamina;
    public float staminaRegain;
    public float maxStamina;
    public bool isKnockedOut = false;
    public bool gettingDamage = false;
    public bool regained = true;
    public bool isCollisionned = false;

    [Header("References")]
    private MyPlayer player;
    public TMP_Text staminaText;

    private static PlayerStamina instance;
    public static PlayerStamina Instance
    {
        get
        {
            if(instance == null) instance = GameObject.FindObjectOfType<PlayerStamina>();
            return instance;
        }
    }

    void Start()
    {
        stamina = maxStamina;
        regained = true;
        player = MyPlayer.Instance;
    }

    void FixedUpdate()
    {
        staminaText.text = stamina + " / " + maxStamina;
        if(stamina <= 0 && !isKnockedOut)
        {
            isKnockedOut = true;
            stamina = 0;
        }
        if(stamina < maxStamina && !isCollisionned && regained && !gettingDamage)
        {
            if(isKnockedOut)
                StartCoroutine(RegainStamina());
            else if(player._moveInput.x == 0)
                StartCoroutine(RegainStamina());
        }
        if(stamina >= maxStamina && !isCollisionned && !gettingDamage)
        {
            stamina = 100;
            if(isKnockedOut)
                isKnockedOut = false;
            regained = true;
        }
    }

    public IEnumerator RegainStamina()
    {
        regained = false;
        stamina += staminaRegain;
        yield return new WaitForSeconds(0.1f);
        regained = true;
        player.SetFrozen(true);
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamageR(damage));
    }
    IEnumerator TakeDamageR(float damage)
    {
        gettingDamage = true;
        stamina -= damage;
        yield return new WaitForSeconds(1f);
        gettingDamage = false;
    }

}
