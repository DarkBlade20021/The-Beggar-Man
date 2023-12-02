using UnityEngine;

public class EnemySteal : MonoBehaviour
{
    public EnemyHealth enemy;
    public EnemyMovement patrol;
    public bool knowsPlayer;
    public bool stole;
    public string playerTag;
    public float staminaDamage;
    public float healthDamage;
    private PlayerStamina playerStamina;
    private PlayerHealth playerHealth;

    private void Update()
    {
        if(enemy.isDead)
            Destroy(this);
        if(knowsPlayer)
        {
            if(playerStamina.isKnockedOut && playerStamina.lastEnemy == this)
            {
                //Stop CHasing Here:
                //patrol.stoppedChasing = true;
                //patrol.isChasing = false;
            }
        }
        if(playerStamina != null)
        {
            if(playerStamina.isKnockedOut && !stole)
            {
                CoinCounter.Instance.SubtractCoinsPercentage(Random.Range(0, 30));
                stole = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == playerTag)
        {
            playerStamina = collision.GetComponent<PlayerStamina>();
            playerHealth = collision.GetComponent<PlayerHealth>();
            playerStamina.isCollisionned = true;
            if(playerStamina.isCollisionned && playerStamina.stamina <= playerStamina.maxStamina && playerStamina.stamina >= playerStamina.staminaRegain && !playerStamina.gettingDamage)
            {
                playerHealth.TakeDamage(healthDamage);
                playerStamina.TakeDamage(staminaDamage, this.GetComponent<EnemySteal>());
            }
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