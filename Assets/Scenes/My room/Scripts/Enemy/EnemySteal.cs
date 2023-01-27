using UnityEngine;

public class EnemySteal : MonoBehaviour
{
    public EnemyHealth enemy;
    public EnemyPatrol patrol;
    public bool knowsPlayer;
    public string playerTag;
    public float staminaDamage;
    private PlayerStamina player;

    private void Update()
    {
        if(enemy.isDead)
            Destroy(this);
        if(knowsPlayer)
        {
            if(player.isKnockedOut)
                patrol.stopFollowing = true;
        }
        if(player.isKnockedOut)
            CoinCounter.Instance.SubtractCoinsPercentage(Random.Range(0, 30));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == playerTag)
        {
            player = collision.GetComponent<PlayerStamina>();
            player.isCollisionned = true;
            if(player.isCollisionned && player.stamina <= player.maxStamina && player.stamina >= player.staminaRegain && !player.gettingDamage)
                player.TakeDamage(staminaDamage);
            knowsPlayer = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == playerTag)
        {
            PlayerStamina player = collision.GetComponent<PlayerStamina>();
            player.isCollisionned = false;
        }
    }
}