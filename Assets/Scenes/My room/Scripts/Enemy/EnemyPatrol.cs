//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class EnemyPatrol : MonoBehaviour
//{
//
//    [Header("Properties")]
//    const string L = "left";
//    const string R = "right";
//
//    [SerializeField] Transform castPos;
//    [SerializeField] public Transform playerTarget;
//    [SerializeField] float baseCastXDist;
//    [SerializeField] float baseCastYDist;
//    public LayerMask groundLayer;
//    public LayerMask playerLayer;
//
//    public float speed;
//    public float chaseSpeed;
//    public bool isFollowing;
//    public bool inTrap;
//    public bool stopFollowing;
//
//    string facingDirection;
//
//    Vector3 baseScale;
//    [SerializeField] Transform platformEdge1;
//    [SerializeField] Transform platformEdge2;
//
//    [Header("References")]
//    public GameObject graphics;
//    public Collider2D collider;
//    public Animator anim;
//    public Rigidbody2D rb;
//    public EnemyHealth health;
//
//    private void Start()
//    {
//        baseScale = transform.localScale;
//
//        facingDirection = L;
//
//        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
//    }
//
//    void FixedUpdate()
//    {
//        anim.SetBool("isFollowing", isFollowing);
//        if(!health.isDead && !isFollowing && !inTrap)
//        {
//            float vX = speed;
//
//            if(facingDirection == L)
//                vX = -speed;
//            else if(facingDirection == R)
//                vX = speed;
//
//            rb.velocity = new Vector2(vX, rb.velocity.y);
//
//            if(IsHittingWall() || IsNearEdge())
//            {
//                if(facingDirection == L)
//                    ChangeFacingDirection(R);
//                else if(facingDirection == R)
//                    ChangeFacingDirection(L);
//            }
//            isFollowing = LookForPlayer();
//        }
//        if(!health.isDead && isFollowing && !inTrap)
//        {
//            if(platformEdge1.gameObject.activeSelf)
//            {
//                platformEdge1.gameObject.SetActive(false);
//                platformEdge2.gameObject.SetActive(false);
//            }
//            float vX = chaseSpeed;
//
//            if(facingDirection == L)
//                vX = -chaseSpeed;
//            else if(facingDirection == R)
//                vX = chaseSpeed;
//
//            rb.velocity = new Vector2(vX, rb.velocity.y);
//
//            if(playerTarget.position.x > transform.position.x && facingDirection == L)
//                ChangeFacingDirection(R);
//            else if(playerTarget.position.x < transform.position.x && facingDirection == R)
//                ChangeFacingDirection(L);
//        }
//        if(!health.isDead && stopFollowing)
//        {
//            float vX = chaseSpeed;
//
//            if(facingDirection == L)
//                vX = chaseSpeed;
//            else if(facingDirection == R)
//                vX = -chaseSpeed;
//
//            rb.velocity = new Vector2(vX, rb.velocity.y);
//
//            if(playerTarget.position.x < transform.position.x && facingDirection == L)
//                ChangeFacingDirection(R);
//            else if(playerTarget.position.x > transform.position.x && facingDirection == R)
//                ChangeFacingDirection(L);
//            isFollowing = true;
//        }
//        if(inTrap)
//        {
//            float vX = chaseSpeed;
//            rb.velocity = new Vector2(vX, rb.velocity.y);
//
//            if(collider.enabled)
//            {
//                this.GetComponent<CircleCollider2D>().enabled = false;
//                this.GetComponent<Rigidbody2D>().gravityScale = 0;
//                collider.enabled = false;
//            }
//        }
//    }
//
//
//    bool LookForPlayer()
//    {
//        bool val = false;
//        float castDist = baseCastXDist;
//
//        if(facingDirection == L)
//            castDist = -baseCastXDist;
//        else if(facingDirection == R)
//            castDist = baseCastXDist;
//
//        if(Physics2D.Linecast(castPos.position, platformEdge1.position, playerLayer) || Physics2D.Linecast(castPos.position, platformEdge2.position, playerLayer))
//            val = true;
//        else
//            val = false;
//
//        return val;
//    }
//
//    bool IsHittingWall()
//    {
//        bool val = false;
//        float castDist = baseCastXDist;
//
//        if(facingDirection == L)
//            castDist = -baseCastXDist;
//        else if(facingDirection == R)
//            castDist = baseCastXDist;
//
//        Vector3 targetPos = castPos.position;
//        targetPos.x += castDist;
//
//        if(Physics2D.Linecast(castPos.position, targetPos, groundLayer))
//            val = true;
//        else
//            val = false;
//
//        return val;
//    }
//
//    bool IsNearEdge()
//    {
//        bool val = true;
//        float castDist = baseCastYDist;
//
//        Vector3 targetPos = castPos.position;
//        targetPos.y -= castDist;
//
//        if(Physics2D.Linecast(castPos.position, targetPos, groundLayer))
//            val = false;
//        else
//            val = true;
//
//        return val;
//}
//
//}

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
    Vector3 baseScale;
    string facingDirection;

    [Header("States")]
    public string currentState = null;
    public string chasingState = "chasing";
    public string notChasingState = "notChasing";
    public string stopChasingState = "stopChasing";

    public Animator anim;
    public Transform target;

    void Start()
    {
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
        if(currentState == notChasingState)
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
        Destroy(this.gameObject);
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
        if(currentState == chasingState)
        {
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
}
