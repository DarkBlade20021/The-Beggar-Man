using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Properties")]
    public float health;
    public float maxHealth;
    public bool isDead = false;

    [Header("References")]
    public EnemyPatrol enemy;
    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    void Start()
    {
        health = maxHealth;
    }

    void FixedUpdate()
    {
        //print(health + " / " + maxHealth + " HP");
        if(health <= 0 && !isDead)
            isDead = enemy.Die(isDead);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(collider.gameObject.tag == "Player" && isDead)
        {
            visualCue.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(collider.gameObject.tag == "Player" && isDead)
        {
            visualCue.SetActive(false);
        }
    }


}
