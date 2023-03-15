using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public int enemiesAlive;
    public int round;

    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive <= 0)
        {
            round++;
            NextWave(round);
        }
    }

    public void NextWave(int numRound)
    {
        for (int num = 0; num <= numRound; num++)
        {
            int numRandom = Random.Range(0,5);
            Debug.Log(numRandom);
            Instantiate(enemyPrefab,spawnPoints[numRandom].transform.position,Quaternion.identity);
            enemiesAlive++;
        }
    }
}