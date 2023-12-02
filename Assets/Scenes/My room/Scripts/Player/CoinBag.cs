using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : MonoBehaviour
{
    [Header("Properties")]
    public float destoryTime;
    public CoinBagItem Data;
    public bool contacted;
    public Quaternion throwRightRot;
    public Quaternion throwLeftRot;

    [Header("References")]
    public AudioSource[] dropSfxs;
    public GameObject mainObject;
    public ParticleSystem destroyPS;
    public SpriteRenderer[] ropes;
    public Animator anim;
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
        if(contacted)
            StartCoroutine(DestroyR());
        if(MyPlayer.Instance.IsFacingRight && !contacted)
            this.transform.rotation = Quaternion.Lerp(transform.rotation, throwRightRot, Data.time);
        else if(!MyPlayer.Instance.IsFacingRight && !contacted)
            this.transform.rotation = Quaternion.Lerp(transform.rotation, throwLeftRot, Data.time);
    }

    IEnumerator DestroyR()
    {
        yield return new WaitForSeconds(destoryTime);
        destroyPS.Play();
        anim.SetBool("isDestroyed", true);
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
