using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyMovement : MonoBehaviour
{
    public GameObject noticeObj;
    public CapsuleCollider2D healthCollider;
    public GameObject[] deathObjs;
    public EnemyHealth health;

    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float chaseSpeed = 5f;
    public float noticeTime;
    public bool noticed;

    public Animator anim;
    private Transform target;
    public bool isChasing = false;
    public bool stoppedChasing = false;
    public bool isFacingRight = true;
    private Vector3 originalScale;

    void Start()
    {
        target = pointA;
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isChasing && !stoppedChasing)
            anim.SetBool("isFollowing", false);
        else
            anim.SetBool("isFollowing", true);
    }

    void FixedUpdate()
    {
        if (!isChasing && !stoppedChasing)
        {
            // Patrol between point A and point B
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);

            //// Flip the sprite if moving towards pointB
            //if (transform.position.x < target.position.x)
            //{
            //    Flip();
            //}
            //else
            //{
            //    Flip();
            //}

            // Switch target points when the enemy reaches one of them
            if (Vector3.Distance(transform.position, new Vector3(pointA.position.x, pointA.position.y, transform.position.z)) < 0.5f)
            {
                target = pointB;
                Flip();
            }
            else if (Vector3.Distance(transform.position, new Vector3(pointB.position.x, pointB.position.y, transform.position.z)) < 0.5f)
            {
                target = pointA;
                Flip();
            }

        }
        else
        {
            if (!noticed)
                StartCoroutine(Notice());

            Vector3 chaseTarget;

            if (isChasing)
            {
                chaseTarget = target.position;
            }
            else
            {
                chaseTarget = new Vector3(-target.position.x, -target.position.y, transform.position.z);
            }

            transform.position = Vector3.MoveTowards(transform.position, chaseTarget, chaseSpeed * Time.deltaTime);

            // Flip the sprite if moving towards the target
            if (transform.position.x < chaseTarget.x && !isFacingRight)
            {
                Flip();
            }
            else if (transform.position.x > chaseTarget.x && isFacingRight)
            {
                Flip();
            }
        }
    }

    IEnumerator Notice()
    {
        noticeObj.SetActive(true);
        yield return new WaitForSeconds(noticeTime);
        noticeObj.SetActive(false);
        noticed = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Start chasing the player if they enter the trigger
        if (other.tag == "Player")
        {
            isChasing = true;
            target = other.transform;
        }
    }

    public bool Die(bool isDead)
    {
        isDead = true;
        StartCoroutine(DieR());
        return isDead;
    }

    IEnumerator DieR()
    {
        anim.SetBool("dead", true);
        anim.SetBool("isFollowing", false);
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<CircleCollider2D>().enabled = false;
        healthCollider.enabled = false;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach (GameObject deathObj in deathObjs)
            Destroy(deathObj);
    }

    private void Flip()
    {  
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
