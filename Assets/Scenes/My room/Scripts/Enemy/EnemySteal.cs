using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySteal : MonoBehaviour
{
    public EnemyHealth enemy;
    public EnemyPatrol patrol;
    public bool knowsPlayer;
    public string playerTag;
    public float staminaDamage;
    private PlayerStamina player;

    private void Update()
    {
        if(enemy.isDead)
            Destroy(this);
        if(knowsPlayer)
        {
            if(player.isKnockedOut)
                patrol.stopFollowing = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == playerTag && !knowsPlayer)
        {
            player = collision.GetComponent<PlayerStamina>();
            if(!player.isCollisionned && player.stamina <= player.maxStamina && player.stamina >= player.staminaRegain)
                player.TakeDamage(staminaDamage);
            knowsPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == playerTag)
        {
            PlayerStamina player = collision.GetComponent<PlayerStamina>();
            player.isCollisionned = false;
        }
    }
}