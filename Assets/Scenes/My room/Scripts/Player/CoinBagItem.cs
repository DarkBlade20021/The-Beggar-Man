using UnityEngine;

[CreateAssetMenu(menuName = "New Coin Bag")]
public class CoinBagItem : ScriptableObject
{
    public float shootRange;
    public int cost;
    public float time;
    public float damage;
    public Vector3 scale;
    public Color color;
}
