using System.Collections;
using UnityEngine;
using TMPro;

public class Trap : MonoBehaviour
{
    public TMP_Text trapWarning;
    private MyPlayer player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            StartCoroutine(StealRoutine(collision));
    }

    IEnumerator StealRoutine(Collider2D collision)
    {
        player = collision.GetComponent<MyPlayer>();
        player.IsFrozen = true;
        Debug.Log(player.IsFrozen);
        trapWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        CoinCounter.Instance.SubtractCoinsPercentage(Random.Range(10, 30));
        player.IsFrozen = false;
        trapWarning.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
