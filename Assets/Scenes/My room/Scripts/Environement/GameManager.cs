using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int levelNum;
    public float minWindTime;
    public float maxWindTime;
    bool isWaitingForWind;
    public GameObject windPrefab;
    public GameObject lightWindPrefab;
    public GameObject enemyPrefab;
    public Transform[] enemyPoses;
    public Transform[] windPoses;

    private void Start()
    {
        levelNum = SceneManager.GetActiveScene().buildIndex;
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if(instance == null) instance = GameObject.FindObjectOfType<GameManager>();
            return instance;
        }
    }

    private void Update()
    {
        if(!isWaitingForWind)
        {
            isWaitingForWind = true;
            float windTime = Random.Range(minWindTime, maxWindTime);
            StartCoroutine(DoWind(windTime));
        }
    }

    IEnumerator DoWind(float windTime)
    {
        int whichWind = Random.Range(0, 2);
        if(whichWind == 0)
        {
            yield return new WaitForSeconds(windTime);
            int windPosI = Random.Range(0, windPoses.Length);
            Instantiate(windPrefab, windPoses[windPosI].position, windPoses[windPosI].rotation);
            isWaitingForWind = false;
        } else if (whichWind == 1)
        {
            yield return new WaitForSeconds(windTime);
            int windPosI = Random.Range(0, windPoses.Length);
            Instantiate(lightWindPrefab, windPoses[windPosI].position, windPoses[windPosI].rotation);
            isWaitingForWind = false;
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

    public void Restart()
    {
        Time.timeScale = 1f;
        MyPlayer.Instance.IsFrozen = false;
        SceneManager.LoadScene(levelNum, LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }

}
