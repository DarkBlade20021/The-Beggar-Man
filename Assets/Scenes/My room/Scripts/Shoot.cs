using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float shootRange;
    public bool canShoot;
    public int bagsInstantiated;
    public Transform GunHolder;
    public Transform shootPoint;
    public GameObject bag;
    public Vector2 direction;


    // Update is called once per frame
    void Update()
    {
        while(canShoot && bagsInstantiated == 0)
        {
            GameObject newBag = Instantiate(bag, shootPoint.position, shootPoint.rotation);
            newBag.transform.localScale = shootPoint.localScale;
            Rigidbody2D rb = newBag.GetComponent<Rigidbody2D>();
            if (MyPlayer.Instance.IsFacingRight)
                rb.AddForce(direction * shootRange);
            else
                rb.AddForce(new Vector2(-direction.x, direction.y) * shootRange);
            bagsInstantiated++;
        }
        if (!canShoot)
            bagsInstantiated = 0;
        if (Input.GetMouseButtonDown(1) && !MyPlayer.Instance.anim.GetBool("isThrowing"))
            ShootBag();
    }

    void ShootBag()
    {
        MyPlayer.Instance.SetThrowing();
        CoinCounter.Instance.SubtractCoins();
    }

}
