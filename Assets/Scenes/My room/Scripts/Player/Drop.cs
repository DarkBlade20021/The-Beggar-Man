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
	
	private void FixedUpdate()
	{
        if(!MyPlayer.Instance.IsFrozen)
        {
            MyPlayer.Instance.dropAction.Enable();
            MyPlayer.Instance.dropAction.performed += ctx => ToDrop();
        }
        else
            MyPlayer.Instance.dropAction.Disable();
        if(!canDrop && !MyPlayer.Instance.IsFrozen)
			canDrop = true;
	}
	
	void ToDrop()
	{
        if(CoinCounter.Instance.Coins >= CoinCounter.Instance.CoinsInBag && canDrop)
        {
			InstantiateBag();
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
