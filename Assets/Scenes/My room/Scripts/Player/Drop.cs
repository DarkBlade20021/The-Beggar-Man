using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Properties")]
    public bool canDrop;
    public bool dropping;
    public int bagsInstantiated;
    public int bagsInstantiating;

    [Header("References")]
    public Transform GunHolder;
    public Transform shootPoint;
    public GameObject bag;


    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Q) && CoinCounter.Instance.Coins >= CoinCounter.Instance.CoinsInBag)
        {
            InstantiateBag();
        }
    }

    public void InstantiateBag()
    {
        GameObject newBag = Instantiate(bag, shootPoint.position, shootPoint.rotation);
        newBag.transform.localScale = shootPoint.localScale;
        CoinCounter.Instance.SubtractCoins();
    }

}
