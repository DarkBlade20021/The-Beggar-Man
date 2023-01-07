using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChangeDirection : MonoBehaviour
{
    public EnemyPatrol enemy;
    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy.graphics.transform.localScale = new Vector2(-enemy.graphics.transform.localScale.x, enemy.graphics.transform.localScale.y);
    }
}