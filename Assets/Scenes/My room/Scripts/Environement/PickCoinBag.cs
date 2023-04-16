using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCoinBag : MonoBehaviour
{
    [SerializeField] Transform coinBagTrans;
    [SerializeField] Transform visualCue;

    private bool playerInRange;

    private void Update()
    {
        visualCue.gameObject.SetActive(playerInRange);
        if(playerInRange)
        {
            MyPlayer.Instance.interactAction.Enable();
            MyPlayer.Instance.interactAction.performed += ctx => PickUp();
        }
        else
        {
            MyPlayer.Instance.interactAction.Disable();
        }
    }

    private void LateUpdate()
    {
        visualCue.position = new Vector3(coinBagTrans.position.x, coinBagTrans.position.y + 2.82f, coinBagTrans.position.z);
    }

    void PickUp()
    {
        CoinCounter.Instance.AddCoins(CoinCounter.Instance.CoinsInBag);
        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
            playerInRange = false;
    }

}
