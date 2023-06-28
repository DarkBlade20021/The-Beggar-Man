using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : MonoBehaviour
{
    [Header("Properties")]
    public CoinBagItem Data;
    public bool contacted;
    public Quaternion throwRightRot;
    public Quaternion throwLeftRot;

    [Header("References")]
    public AudioSource[] dropSfxs;
    public GameObject mainObject;
    public SpriteRenderer[] ropes;
    public Rigidbody2D rb;
    public string[] groundTags;
    public string[] enemyTags;

    void Start()
    {
        Data = Inventory.Instance.currentBag;
        mainObject.transform.localScale = Data.scale;
        foreach(SpriteRenderer rope in ropes)
            rope.color = Data.color;
    }

    private void Update()
    {
        if(MyPlayer.Instance.IsFacingRight && !contacted)
            mainObject.transform.rotation = Quaternion.Lerp(transform.rotation, throwRightRot, Data.time);
        else if(!MyPlayer.Instance.IsFacingRight && !contacted)
            mainObject.transform.rotation = Quaternion.Lerp(transform.rotation, throwLeftRot, Data.time);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string groundTag in groundTags)
        {
            if(collision.gameObject.tag == groundTag && !contacted)
            {
                int dropSfx = Random.Range(0, dropSfxs.Length);
                dropSfxs[dropSfx].Play();
                contacted = true;
            }
        }
        foreach(string enemyTag in enemyTags)
        {
            if(collision.gameObject.tag == enemyTag && !contacted)
            {
                int dropSfx = Random.Range(0, dropSfxs.Length);
                dropSfxs[dropSfx].Play();
                collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(Data.damage);
                contacted = true;
            }
        }
    }
}
