using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Properties")]
public class EnemyProp : ScriptableObject
{
    public bool isBoss;
    public float moveSpeed = 2f;
    public float chaseSpeed = 5f;
    public float maxHealth;
    public Vector3 scale;
}
