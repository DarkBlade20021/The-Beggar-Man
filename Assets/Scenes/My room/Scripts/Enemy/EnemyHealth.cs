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
    public EnemyMovement enemy;
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    public bool playerInRange;

    void Start()
    {
        visualCue.SetActive(false);
        health = maxHealth;
    }


    void FixedUpdate()
    {
        if(MyPlayer.Instance.interactAction.WasReleasedThisFrame() && !looted)
            ToInteract();
        print(health + " / " + maxHealth + " HP");
        if(health <= 0 && !isDead)
            isDead = enemy.Die(isDead);
    }

    void ToInteract()
    {
        CoinCounter.Instance.AddCoins(100);
        looted = true;
        Destroy(enemy.gameObject);
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

    public bool playerHere()
    {
        return playerInRange;
    }

}
