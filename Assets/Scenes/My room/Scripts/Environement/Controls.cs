using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public Animator anim;
    bool showed;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!showed && other.gameObject.tag == "Player")
        {
            anim.Play("ControlShow");
            showed = true;
        }
    }
}
