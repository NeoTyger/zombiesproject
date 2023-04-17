using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public int enemiesAlive;
    public int round;

    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;

    public TextMeshProUGUI txtHp;
    public TextMeshProUGUI txtRound;

    public GameObject pausePanel;

    public bool isPaused;
    public bool isGameOver;
    
    public GameObject gameOverPanel;

    public static GameManager sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1;

        spawnPoints = GameObject.FindGameObjectsWithTag("Spawners");
    }

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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
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

        isPaused = true;
    }
    
    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;

        isPaused = false;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isGameOver = true;
    }
}
