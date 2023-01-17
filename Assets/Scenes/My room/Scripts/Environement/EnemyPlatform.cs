using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatform : MonoBehaviour
{
    public bool isTriggered;
    public float time;

    public Transform newPos;
    public Transform oldPos;

    private void Update()
    {
        if(isTriggered)
            transform.position = Vector2.Lerp(transform.position, newPos.position, time);
        else
            transform.position = Vector2.Lerp(transform.position, oldPos.position, time);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
            isTriggered = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
            isTriggered = false;
    }
}
