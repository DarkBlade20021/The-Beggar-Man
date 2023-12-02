using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    [HideInInspector] public EnemyProp Data;

    private bool stoppedChasing = false;
    private bool isFacingRight = true;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!stoppedChasing)
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        Transform target = isFacingRight ? pointA : pointB;
        Vector2 targetPosition = Vector2.MoveTowards(rb.position, target.position, Data.moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(targetPosition);

        if (Vector2.Distance(rb.position, target.position) < 0.5f)
        {
            SwapTarget();
        }
    }

    private void SwapTarget()
    {
        isFacingRight = !isFacingRight;
        Flip();
    }

    private void Flip()
    {
        transform.Rotate(Vector3.up, 180f);
    }
}
