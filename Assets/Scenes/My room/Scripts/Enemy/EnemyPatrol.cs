using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public GameObject noticeObj;
    public CapsuleCollider2D healthCollider;
    public GameObject[] deathObjs;
    public EnemyHealth health;
    public Animator anim;

    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    public float chaseSpeed = 5f;
    public float noticeTime;
    public bool noticed;

    private Transform target;
    public bool isChasing = false;
    public bool stoppedChasing = false;
    private bool isFacingRight = true;
    private Vector3 originalScale;

    void Start()
    {
        target = pointA;
        originalScale = transform.localScale;
    }

    void Update()
	{
		if (!isChasing && !stoppedChasing)
		{
			anim.SetBool("isFollowing", false);
			// Patrol between point A and point B
			transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
	
			// Flip the sprite if moving towards the next point
			if (transform.position.x < target.position.x && !isFacingRight)
			{
				Flip();
			}
			else if (transform.position.x > target.position.x && isFacingRight)
			{
				Flip();
			}
	
			// Switch target points when the enemy reaches one of them
			if (Vector2.Distance(transform.position, target.position) < 0.1f)
			{
				if (target == pointA)
					target = pointB;
				else
					target = pointA;
			}
		}
		else if (!isChasing && stoppedChasing)
		{
			if (!noticed)
				StartCoroutine(Notice());
	
			anim.SetBool("isFollowing", true);
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(-target.position.x, -target.position.y, target.position.z), chaseSpeed * Time.deltaTime);
	
			// Flip the sprite if moving towards the target
			if (transform.position.x > target.position.x && !isFacingRight)
			{
				Flip();
			}
			else if (transform.position.x < target.position.x && isFacingRight)
			{
				Flip();
			}
		}
		else if (isChasing)
		{
			if (!noticed)
				StartCoroutine(Notice());
	
			anim.SetBool("isFollowing", true);
			transform.position = Vector2.MoveTowards(transform.position, target.position, chaseSpeed * Time.fixedDeltaTime);
	
			// Flip the sprite if moving towards the target
			if (transform.position.x < target.position.x && !isFacingRight)
			{
				Flip();
			}
			else if (transform.position.x > target.position.x && isFacingRight)
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
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<CircleCollider2D>().enabled = false;
        healthCollider.enabled = false;
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach (GameObject deathObj in deathObjs)
            Destroy(deathObj);
        anim.SetBool("dead", true);
        anim.SetBool("isFollowing", false);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 newScale = originalScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
