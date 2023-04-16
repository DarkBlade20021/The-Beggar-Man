using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerStamina.Instance.stamina = 0;
            PlayerHealth.Instance.TakeDamage(100);
        }
    }
}
