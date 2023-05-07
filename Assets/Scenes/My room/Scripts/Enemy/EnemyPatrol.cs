using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Properties")]
    const string L = "left";
    const string R = "right";

    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 10f;
    public float chaseSpeed = 10f;
    public float noticeTime;
    public bool noticed;
    Vector3 baseScale;
    string facingDirection;

    [Header("States")]
    public string currentState = null;
    public string chasingState = "chasing";
    public string notChasingState = "notChasing";
    public string stopChasingState = "stopChasing";

    public Animator anim;
    public Transform target;
    public GameObject noticeObj;
    public GameObject[] deathObjs;

    void Start()
    {
        noticeObj.SetActive(false);
        baseScale = transform.localScale;
        facingDirection = L;
        target = pointA;
        currentState = notChasingState;
    }

    void ChangeFacingDirection(string newDirection)
    {
        Vector3 newScale = baseScale;

        if(newDirection == L)
            newScale.x = -baseScale.x;
        else if(newDirection == R)
            newScale.x = baseScale.x;

        transform.localScale = newScale;

        facingDirection = newDirection;
    }

    void Update()
    {
        if(currentState == notChasingState && !isDead)
        {
            anim.SetBool("isFollowing", false);
            // Patrol between point A and point B
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Flip the sprite if moving towards pointB
            if(transform.position.x < target.position.x)
            {
                ChangeFacingDirection(R);
            }
            else
            {
                ChangeFacingDirection(L);
            }

            // Switch target points when the enemy reaches one of them
            if(Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                target = pointB;
            }
            else if(Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                target = pointA;
            }
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
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<CircleCollider2D>().enabled = false;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach (GameObject deathObj in deathObjs)
            Destroy(deathObj);
        anim.SetBool("dead", true);
        anim.SetBool("isFollowing", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Start chasing the player if they enter the enemy's territory
        if(other.tag == "Player")
        {
            currentState = chasingState;
            target = other.transform;
        }
    }

    private void FixedUpdate()
    {
        // Chase the player if they are in the enemy's territory
        if(currentState == chasingState && !isDead)
        {
            if(!noticed)
                StartCoroutine(Notice());

            anim.SetBool("isFollowing", true);
            transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed * Time.fixedDeltaTime);

            // Flip the sprite if moving towards the player
            if(transform.position.x < target.position.x)
            {
                ChangeFacingDirection(R);
            }
            else
            {
                ChangeFacingDirection(L);
            }
        } else if(currentState == stopChasingState)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(-target.position.x, -target.position.y, target.position.z), chaseSpeed * Time.fixedDeltaTime);

            // Flip the sprite if moving towards the player
            if(transform.position.x > target.position.x)
            {
                ChangeFacingDirection(R);
            }
            else
            {
                ChangeFacingDirection(L);
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
}
