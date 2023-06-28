using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject noticeObj;
    public CapsuleCollider2D healthCollider;
    public GameObject[] deathObjs;
    public EnemyHealth health;
    public EnemyProp Data;

    public Transform pointA;
    public Transform pointB;
    public float noticeTime;
    public bool noticed;

    public Animator anim;
    private Transform target;
    public bool isChasing = false;
    public bool stoppedChasing = false;
    public string right = "Right";
    public string left = "Left";
    public bool isFacingRight = true;
    private Vector3 originalScale;

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;
    void Start()
    {
        target = pointA;
        print("Before: " + transform.localScale);
        transform.localScale = Data.scale;
        originalScale = transform.localScale;
        print("After: " + transform.localScale);
    }

    void Update()
    {
        visualCue.SetActive(health.playerHere());
        if(!health.isDead)
        {
            if (!isChasing && !stoppedChasing)
                anim.SetBool("isFollowing", false);
            else
                anim.SetBool("isFollowing", true);
        }
    }

    void FixedUpdate()
    {
        if(!health.isDead)
        {
            if (!isChasing && !stoppedChasing )
            {
                // Patrol between point A and point B
                transform.position = Vector3.MoveTowards(transform.position, new    Vector3(target.position.x, target.position.y, -16.5f), Data.moveSpeed *     Time.deltaTime);

                // Switch target points when the enemy reaches one of them
                if (Vector3.Distance(transform.position, new Vector3(pointA.    position.x, pointA.position.y, -16.5f)) < 0.5f)
                {
                    target = pointB;
                    Flip(left);
                }
                else if (Vector3.Distance(transform.position, new Vector3(pointB.   position.x, pointB.position.y, -16.5f)) < 0.5f)
                {
                    target = pointA;
                    Flip(right);
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
                    chaseTarget = new Vector3(-target.position.x, -target.position. y, -16.5f);
                }

                transform.position = Vector3.MoveTowards(transform.position,    chaseTarget, Data.chaseSpeed * Time.deltaTime);

                // Flip the sprite if moving towards the target
                if (transform.position.x < chaseTarget.x && !isFacingRight)
                {
                    Flip(right);
                }
                else if (transform.position.x > chaseTarget.x && isFacingRight)
                {
                    Flip(left);
                }
            }
        }
    }

    IEnumerator Notice()
    {
        noticed = true;
        noticeObj.SetActive(true);
        yield return new WaitForSeconds(noticeTime);
        noticeObj.SetActive(false);
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

    private void Flip(string direction)
    {  
        if(direction == right)
        {
            isFacingRight = true;
            transform.rotation = new Quaternion(0f, 0f, 0f, 1f);
        }
        else if(direction == left)
        {
            isFacingRight = false;
            transform.rotation = new Quaternion(0f, 180f, 0f, 1f);
        }
    }
}
