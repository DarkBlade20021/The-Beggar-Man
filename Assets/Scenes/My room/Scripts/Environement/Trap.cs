using System.Collections;
using UnityEngine;
using TMPro;

public class Trap : MonoBehaviour
{
    public TMP_Text trapWarning;
    public GameObject enemyPrefab;
    public Transform enemyPos;
    public bool stole;
    public float time;
    public Transform newPos;
    private MyPlayer player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            StartCoroutine(StealRoutine(collision));
    }

    IEnumerator StealRoutine(Collider2D collision)
    {
        this.player = collision.GetComponent<MyPlayer>();
        this.player.IsFrozen = true;
        Debug.Log(this.player.IsFrozen);
        trapWarning.gameObject.SetActive(true);
        GameObject obj = Instantiate(enemyPrefab, enemyPos.position, enemyPos.rotation);
        obj.transform.position = Vector3.Lerp(transform.position, newPos.position, time);
        PlayerStamina player = PlayerStamina.Instance;
        player.lastEnemy = obj.GetComponentInChildren<EnemySteal>();
        obj.GetComponent<EnemyPatrol>().target = collision.gameObject.transform;
        player.stamina = 0;
        if(!stole)
        {
            CoinCounter.Instance.SubtractCoinsPercentage(Random.Range(10, 30));
            stole = true;
        }
        yield return new WaitForSeconds(3f);
        this.player.IsFrozen = false;
        trapWarning.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        Destroy(obj);
    }

}
