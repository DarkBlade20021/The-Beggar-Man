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
    public bool regained = true;
    public bool isCollisionned = false;

    [Header("References")]
    private MyPlayer player;
    public TMP_Text staminaText;

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
        if(stamina < maxStamina && !isCollisionned && regained)
        {
            if(isKnockedOut)
            {
                StartCoroutine(RegainStamina());
                player.SetFrozen();
            }
            else if(player._moveInput.x == 0)
                StartCoroutine(RegainStamina());
        }
        if(stamina >= maxStamina && !isCollisionned)
        {
            stamina = 100;
            if(isKnockedOut)
            {
                isKnockedOut = false;
                player.SetUnfrozen();
            }
            regained = true;
        }
    }

    public IEnumerator RegainStamina()
    {
        regained = false;
        stamina += staminaRegain;
        yield return new WaitForSeconds(0.1f);
        regained = true;
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamageR(damage));
    }
    IEnumerator TakeDamageR(float damage)
    {
        isCollisionned = true;
        stamina -= damage;
        yield return new WaitForSeconds(1f);
        isCollisionned = false;
    }

}
