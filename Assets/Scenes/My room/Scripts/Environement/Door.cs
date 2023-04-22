using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PressurePlate[] pressurePlates;
    public float time;
    public bool openned;
    public Transform newPos;
    public Transform oldPos;
	public GameObject smoke;
    public int pressurePlatesOpenned = 0;
    
    void Start()
    {
        oldPos.position = transform.position;
    }
    
    void Update()
    {
        smoke.SetActive(openned);
        if(pressurePlatesOpenned == pressurePlates.Length && !openned)
			openned = true;
        else if (pressurePlatesOpenned < pressurePlates.Length && !openned)
            transform.position = Vector3.Lerp(transform.position, oldPos.position, time * 0.75f);
        else if (openned)
        {
            transform.position = Vector3.Lerp(transform.position, newPos.position, time * 0.1f);
        }
    }
    
}
