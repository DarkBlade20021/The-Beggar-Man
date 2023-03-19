using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isPushed;
    public bool isToggle;
    public float time;
    public GameObject obj;
    public Transform newPos;
    public Transform oldPos;
    public Door door;
    
    void Start()
    {
        oldPos.position = obj.transform.position;
    }
    
    void Update()
    {
        if(isPushed)
            obj.transform.position = Vector3.Lerp(transform.position, newPos.position, time);
        else
            obj.transform.position = Vector3.Lerp(transform.position, oldPos.position, time);
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin Bag" && !isPushed || other.gameObject.tag == "Player" && !isPushed)
        {
            isPushed = true;
            door.pressurePlatesOpenned++;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Coin Bag" && isPushed && !isToggle || other.gameObject.tag == "Player" && isPushed && !isToggle)
        {
            isPushed = false;
            door.pressurePlatesOpenned--;
        }
    }
}
