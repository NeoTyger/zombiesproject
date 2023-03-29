using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int enemiesAlive;
    public int round;

    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;

    public TextMeshProUGUI txtHp;
    public TextMeshProUGUI txtRound;

    public GameObject pausePanel;

    // Update is called once per frame
    void Update()
    {
        if (enemiesAlive <= 0)
        {
            round++;
            NextWave(round);
        }

        txtHp.text = "HP : " + PlayerManager.health;
        txtRound.text = "Round : " + round;
    }

    public void NextWave(int numRound)
    {
        for (int num = 0; num <= numRound; num++)
        {
            int numRandom = Random.Range(0,5);
            //Debug.Log(numRandom);
            Instantiate(enemyPrefab,spawnPoints[numRandom].transform.position,Quaternion.identity);
            enemiesAlive++;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
    
    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
