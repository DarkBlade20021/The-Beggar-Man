using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Properties")]
    public float health;
    public float maxHealth;
    public bool isDead = false;
    public bool looted = false;

    [Header("References")]
    public EnemyPatrol enemy;
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    private bool playerInRange;

    void Start()
    {
        visualCue.SetActive(false);
        health = maxHealth;
    }

    void FixedUpdate()
    {
        visualCue.SetActive(playerInRange);
        if(playerInRange && !looted)
        {
            MyPlayer.Instance.interactAction.Enable();
            MyPlayer.Instance.interactAction.performed += ctx => ToInteract();
        }
        else if(!playerInRange || looted)
        {
            MyPlayer.Instance.interactAction.performed -= ctx => ToInteract();
            MyPlayer.Instance.interactAction.Disable();
        }
        //print(health + " / " + maxHealth + " HP");
        if(health <= 0 && !isDead)
            isDead = enemy.Die(isDead);
    }

    void ToInteract()
    {
        CoinCounter.Instance.AddCoins(75);
        looted = true;
    }    

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && isDead)
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && isDead)
            playerInRange = false;
    }


}
