using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] enemyPoses;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public void SpawnWave()
    {
        for(int i = 0; i < enemyPoses.Length; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, enemyPoses[i].position, enemyPoses[i].rotation);
            PlayerStamina player = PlayerStamina.Instance;
            player.lastEnemy = obj.GetComponentInChildren<EnemySteal>();
            obj.GetComponent<EnemyPatrol>().target = GameObject.FindGameObjectWithTag("Player").transform;
            obj.GetComponent<EnemyPatrol>().currentState = obj.GetComponent<EnemyPatrol>().stopChasingState;
        }
    }
}
