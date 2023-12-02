using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject noticeObj;
    public GameObject visualCue;
    public CapsuleCollider2D healthCollider;
    public GameObject[] deathObjs;
    public EnemyHealth health;
    public EnemyProp Data;

    public Animator anim;

    public EnemyChase chase;
    public EnemyPatrol patrol;

    private void Start()
    {
        chase.anim = anim;
        patrol.Data = Data;
        chase.Data = Data;

        // Set initial behavior
        patrol.enabled = true;
        chase.enabled = false;

        // Set other initial configurations
        transform.localScale = Data.scale;
        anim.SetBool("isFollowing", false);
    }

    private void Update()
    {
        visualCue.SetActive(health.playerHere());

        if (!health.isDead)
        {
            anim.SetBool("isFollowing", chase.enabled);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            chase.enabled = true;
            chase.StartChasing(other.transform);
            patrol.enabled = false;
        }
    }

    public bool Die(bool isDead)
    {
        isDead = true;
        StartCoroutine(DieR());
        return isDead;
    }

    private IEnumerator DieR()
    {
        chase.StopChasing();
        anim.SetBool("dead", true);
        anim.SetBool("isFollowing", false);
        yield return new WaitForSeconds(0.2f);
        GetComponent<CircleCollider2D>().enabled = false;
        healthCollider.enabled = false;
        chase.enabled = false;
        patrol.enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        foreach (GameObject deathObj in deathObjs)
            Destroy(deathObj);
    }
}
