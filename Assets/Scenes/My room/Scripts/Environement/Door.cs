using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PressurePlate[] pressurePlates;
    public float time;
    public Transform newPos;
    public Transform oldPos;
    public int pressurePlatesOpenned = 0;
    
    void Start()
    {
        oldPos.position = transform.position;
    }
    
    void Update()
    {
        if(pressurePlatesOpenned == pressurePlates.Length)
            transform.position = Vector3.Lerp(transform.position, newPos.position, time * 0.1f);
        else if (pressurePlatesOpenned < pressurePlates.Length)
            transform.position = Vector3.Lerp(transform.position, oldPos.position, time * 0.75f);
    }
    
}
