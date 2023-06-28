using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Header("Properties")]
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
        if(!MyPlayer.Instance.IsFrozen)
        {
            if(!canShoot)
                bagsInstantiated = 0;
            MyPlayer.Instance.throwAction.Enable();
            MyPlayer.Instance.throwAction.performed += ctx => ToThrow();
        }
        else
        {
            MyPlayer.Instance.throwAction.Disable();
        }
    }

    void ToThrow()
    {
        if(CoinCounter.Instance.Coins >= Inventory.Instance.currentBag.cost && !shooting && bagsInstantiating == 0)
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
        Rigidbody2D rb = newBag.GetComponentInChildren<Rigidbody2D>();
        if(MyPlayer.Instance.IsFacingRight)
            rb.AddForce(direction * Inventory.Instance.currentBag.shootRange);
        else
            rb.AddForce(new Vector2(-direction.x, direction.y) * Inventory.Instance.currentBag.shootRange);
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