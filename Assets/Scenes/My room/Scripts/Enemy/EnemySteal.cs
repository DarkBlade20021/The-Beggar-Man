using UnityEngine;

public class EnemySteal : MonoBehaviour
{
    public EnemyHealth enemy;
    public EnemyPatrol patrol;
    public bool knowsPlayer;
    public bool stole;
    public string playerTag;
    public float staminaDamage;
    private PlayerStamina player;

    private void Update()
    {
        if(enemy.isDead)
            Destroy(this);
        if(knowsPlayer)
        {
            if(player.isKnockedOut && player.lastEnemy == this)
                patrol.stopFollowing = true;
        }
        if(player.isKnockedOut && !stole)
        {
            CoinCounter.Instance.SubtractCoinsPercentage(Random.Range(0, 30));
            stole = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == playerTag)
        {
            player = collision.GetComponent<PlayerStamina>();
            player.isCollisionned = true;
            if(player.isCollisionned && player.stamina <= player.maxStamina && player.stamina >= player.staminaRegain && !player.gettingDamage)
                player.TakeDamage(staminaDamage, this.GetComponent<EnemySteal>());
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