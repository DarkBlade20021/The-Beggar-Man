using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public GameObject noticeObj;
    public float noticeTime;
    public Transform target;
    [HideInInspector] public EnemyProp Data;
    [HideInInspector] public EnemyHealth hp;

    private bool isChasing = false;
    private bool noticed = false;
    public Animator anim;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isChasing)
        {
            ChaseTarget();
        }
    }

    private void ChaseTarget()
    {
        Vector2 targetPosition = Vector2.MoveTowards(rb.position, target.position, Data.chaseSpeed * Time.deltaTime);
        rb.MovePosition(targetPosition);
        if (rb.position.x < target.position.x && isFacingRight && !hp.isDead)
        {
            Flip();
        }
        else if (rb.position.x > target.position.x && !isFacingRight && !hp.isDead)
        {
            Flip();
        }
    }

    public void StartChasing(Transform newTarget)
    {
        if (!noticed)
        {
            noticed = true;
            StartCoroutine(Notice());
        }
        target = newTarget;
        isChasing = true;
        anim.SetBool("isFollowing", true);
    }

    public void StopChasing()
    {
        isChasing = false;
        anim.SetBool("isFollowing", false);
    }

    private IEnumerator Notice()
    {
        noticed = true;
        noticeObj.SetActive(true);
        yield return new WaitForSeconds(noticeTime);
        noticeObj.SetActive(false);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180f);
    }
}
