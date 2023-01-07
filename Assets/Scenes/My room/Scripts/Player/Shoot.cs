using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Properties")]
    public float shootRange;
    public float shootTime;
    public bool canShoot;
    public bool shooting;
    public int bagsInstantiated;
    public int bagsInstantiating;

    [Header("References")]
    public Transform GunHolder;
    public Transform shootPoint;
    public GameObject bag;
    public Vector2 direction;


    // Update is called once per frame
    void FixedUpdate()
    {
        if(!canShoot)
            bagsInstantiated = 0;
        if (Input.GetMouseButtonDown(1) && CoinCounter.Instance.Coins >= CoinCounter.Instance.CoinsInBag && !shooting && bagsInstantiating == 0)
        {
            #region ANIMATION SOLVERS
            MyPlayer.Instance.anim.SetBool("isThrowing", false);
            #endregion
            ShootBag();
            bagsInstantiating++;
        }
    }

    public void InstantiateBag()
    {
        GameObject newBag = Instantiate(bag, shootPoint.position, shootPoint.rotation);
        newBag.transform.localScale = shootPoint.localScale;
        Rigidbody2D rb = newBag.GetComponent<Rigidbody2D>();
        if(MyPlayer.Instance.IsFacingRight)
            rb.AddForce(direction * shootRange);
        else
            rb.AddForce(new Vector2(-direction.x, direction.y) * shootRange);
        bagsInstantiated++;
    }

    void ShootBag()
    {
        shooting = true;
        MyPlayer.Instance.anim.SetBool("isThrowing", false);
        MyPlayer.Instance.SetThrowing();
        CoinCounter.Instance.SubtractCoins();
    }

}
