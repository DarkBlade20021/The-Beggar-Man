using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJumpPad : MonoBehaviour
{
    public float jumpForce;
    public float time;

    public Transform newPos;
    public Transform oldPos;
    public EnemyPlatform platform;

    private void Update()
    {
        if(platform.isTriggered)
            transform.position = Vector2.Lerp(transform.position, newPos.position, time);
        else
            transform.position = Vector2.Lerp(transform.position, oldPos.position, time);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && collision.GetComponent<EnemyPatrol>().isFollowing)
        {
            collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            collision.GetComponent<Animator>().SetTrigger("jump");
        }
    }
}
