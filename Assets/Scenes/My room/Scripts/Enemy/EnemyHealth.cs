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

    void Start()
    {
        health = maxHealth;
    }

    void FixedUpdate()
    {
        print(health + " / " + maxHealth + " HP");
        if(health <= 0 && !isDead)
            isDead = enemy.Die(isDead);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }


}
