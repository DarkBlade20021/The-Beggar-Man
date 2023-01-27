using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Properties")]
    public bool canDrop;

    [Header("References")]
    public Transform GunHolder;
    public Transform shootPoint;
    public GameObject bag;

    private void Start()
    {
        canDrop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!MyPlayer.Instance.IsFrozen)
        {
            if(Input.GetKeyDown(KeyCode.Q) && CoinCounter.Instance.Coins >= CoinCounter.Instance.CoinsInBag && canDrop)
                InstantiateBag();
            if(Input.GetKeyUp(KeyCode.Q) && !canDrop)
                canDrop = true;
        }
    }

    public void InstantiateBag()
    {
        canDrop = false;
        GameObject newBag = Instantiate(bag, shootPoint.position, shootPoint.rotation);
        newBag.transform.localScale = shootPoint.localScale;
        CoinCounter.Instance.SubtractCoins();
    }

}
