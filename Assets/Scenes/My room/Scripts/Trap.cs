using System.Collections;
using UnityEngine;
using TMPro;

public class Trap : MonoBehaviour
{
    public TMP_Text trapWarning;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            StartCoroutine(StealRoutine());
    }

    IEnumerator StealRoutine()
    {
        MyPlayer.Instance.SetFrozen();
        trapWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        CoinCounter.Instance.SubtractCoinsPercentage(Random.Range(10, 30));
        MyPlayer.Instance.SetUnfrozen();
        trapWarning.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
