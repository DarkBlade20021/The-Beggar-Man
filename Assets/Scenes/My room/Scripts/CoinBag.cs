using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBag : MonoBehaviour
{
    [Header("Properties")]
    public bool contacted;
    public float time;
    public Quaternion throwRightRot;
    public Quaternion throwLeftRot;

    [Header("References")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;

    private void Update()
    {
        if(MyPlayer.Instance.IsFacingRight && !contacted)
            transform.rotation = Quaternion.Lerp(transform.rotation, throwRightRot, time);
        else if(!MyPlayer.Instance.IsFacingRight && !contacted)
            transform.rotation = Quaternion.Lerp(transform.rotation, throwLeftRot, time);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == groundLayer && !contacted)
            contacted = true;
    }
}
