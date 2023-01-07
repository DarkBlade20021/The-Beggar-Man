using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : MonoBehaviour
{
    [Header("Properties")]
    public bool contacted;
    public float time;
    public float damage;
    public Quaternion throwRightRot;
    public Quaternion throwLeftRot;

    [Header("References")]
    public GameObject mainObject;
    public Rigidbody2D rb;
    public string[] groundTags;
    public string[] enemyTags;

    private void Update()
    {
        if(MyPlayer.Instance.IsFacingRight && !contacted)
            mainObject.transform.rotation = Quaternion.Lerp(transform.rotation, throwRightRot, time);
        else if(!MyPlayer.Instance.IsFacingRight && !contacted)
            mainObject.transform.rotation = Quaternion.Lerp(transform.rotation, throwLeftRot, time);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string groundTag in groundTags)
        {
            if(collision.gameObject.tag == groundTag && !contacted)
                contacted = true;
        }
        foreach(string enemyTag in enemyTags)
        {
            if(collision.gameObject.tag == enemyTag && !contacted)
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
        }
    }
}
