using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isPushed;
    public float time;
    public Transform newPos;
    public Transform oldPos;
    public Door door;
    
    void Start()
    {
        oldPos.position = transform.position;
    }
    
    void Update()
    {
        if(isPushed)
            transform.position = Vector3.Lerp(transform.position, newPos.position, time);
        else
            transform.position = Vector3.Lerp(transform.position, oldPos.position, time);
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin Bag" || other.gameObject.tag == "Player" && !isPushed)
        {
            isPushed = true;
            door.pressurePlatesOpenned++;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin Bag" || other.gameObject.tag == "Player" && isPushed)
        {
            isPushed = false;
            door.pressurePlatesOpenned--;
        }
    }
}
