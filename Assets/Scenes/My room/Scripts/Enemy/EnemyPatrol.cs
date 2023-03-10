using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    [Header("Properties")]
    const string L = "left";
    const string R = "right";

    [SerializeField] Transform castPos;
    [SerializeField] public Transform playerTarget;
    [SerializeField] float baseCastXDist;
    [SerializeField] float baseCastYDist;
    public LayerMask groundLayer;
    public LayerMask playerLayer;

    public float speed;
    public float chaseSpeed;
    public bool isFollowing;
    public bool inTrap;
    public bool stopFollowing;

    string facingDirection;

    Vector3 baseScale;
    [SerializeField] Transform platformEdge1;
    [SerializeField] Transform platformEdge2;

    [Header("References")]
    public GameObject graphics;
    public Collider2D collider;
    public Animator anim;
    public Rigidbody2D rb;
    public EnemyHealth health;

    private void Start()
    {
        baseScale = transform.localScale;

        facingDirection = R;

        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        anim.SetBool("isFollowing", isFollowing);
        if(!health.isDead && !isFollowing && !inTrap)
        {
            float vX = speed;

            if(facingDirection == L)
                vX = -speed;
            else if(facingDirection == R)
                vX = speed;

            rb.velocity = new Vector2(vX, rb.velocity.y);

            if(IsHittingWall() || IsNearEdge())
            {
                if(facingDirection == L)
                    ChangeFacingDirection(R);
                else if(facingDirection == R)
                    ChangeFacingDirection(L);
            }
            isFollowing = LookForPlayer();
        }
        if(!health.isDead && isFollowing && !inTrap)
        {
            if(platformEdge1.gameObject.activeSelf)
            {
                platformEdge1.gameObject.SetActive(false);
                platformEdge2.gameObject.SetActive(false);
            }
            float vX = chaseSpeed;

            if(facingDirection == L)
                vX = -chaseSpeed;
            else if(facingDirection == R)
                vX = chaseSpeed;

            rb.velocity = new Vector2(vX, rb.velocity.y);

            if(playerTarget.position.x > transform.position.x && facingDirection == L)
                ChangeFacingDirection(R);
            else if(playerTarget.position.x < transform.position.x && facingDirection == R)
                ChangeFacingDirection(L);
        }
        if(!health.isDead && stopFollowing)
        {
            float vX = chaseSpeed;

            if(facingDirection == L)
                vX = chaseSpeed;
            else if(facingDirection == R)
                vX = -chaseSpeed;

            rb.velocity = new Vector2(vX, rb.velocity.y);

            if(playerTarget.position.x < transform.position.x && facingDirection == L)
                ChangeFacingDirection(R);
            else if(playerTarget.position.x > transform.position.x && facingDirection == R)
                ChangeFacingDirection(L);
            isFollowing = true;
        }
        if(inTrap)
        {
            float vX = chaseSpeed;
            rb.velocity = new Vector2(vX, rb.velocity.y);

            if(collider.enabled)
            {
                this.GetComponent<CircleCollider2D>().enabled = false;
                this.GetComponent<Rigidbody2D>().gravityScale = 0;
                collider.enabled = false;
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
        health.GetComponent<CapsuleCollider2D>().enabled = false;
        Destroy(graphics);
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

    bool LookForPlayer()
    {
        bool val = false;
        float castDist = baseCastXDist;

        if(facingDirection == L)
            castDist = -baseCastXDist;
        else if(facingDirection == R)
            castDist = baseCastXDist;

        if(Physics2D.Linecast(castPos.position, platformEdge1.position, playerLayer) || Physics2D.Linecast(castPos.position, platformEdge2.position, playerLayer))
            val = true;
        else
            val = false;

        return val;
    }

    bool IsHittingWall()
    {
        bool val = false;
        float castDist = baseCastXDist;

        if(facingDirection == L)
            castDist = -baseCastXDist;
        else if(facingDirection == R)
            castDist = baseCastXDist;

        Vector3 targetPos = castPos.position;
        targetPos.x += castDist;

        if(Physics2D.Linecast(castPos.position, targetPos, groundLayer))
            val = true;
        else
            val = false;

        return val;
    }

    bool IsNearEdge()
    {
        bool val = true;
        float castDist = baseCastYDist;

        Vector3 targetPos = castPos.position;
        targetPos.y -= castDist;

        if(Physics2D.Linecast(castPos.position, targetPos, groundLayer))
            val = false;
        else
            val = true;

        return val;
    }

}
