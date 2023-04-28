using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : MonoBehaviourPunCallbacks
{

    public int enemiesAlive;
    public int round;

    public GameObject[] spawnPoints;

    public TextMeshProUGUI txtHp;
    public TextMeshProUGUI txtRound;

    public GameObject pausePanel;

    public bool isPaused;
    public bool isGameOver;
    
    public GameObject gameOverPanel;

    public PhotonView photonView;
    
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
        if (!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && photonView.IsMine))
        {
            if (enemiesAlive <= 0)
            {
                round++;
                NextWave(round);

                if (PhotonNetwork.InRoom)
                {
                    Hashtable hash = new Hashtable();
                    hash.Add("currentRound", round);
                    PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
                }
                else
                {
                    DisplayNextRound(round);
                }
            }
        }

        txtHp.text = "HP : " + PlayerManager.health;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    private void DisplayNextRound(int round)
    {
        txtRound.text = "Round : " + round;
    }

    public void NextWave(int numRound)
    {
        for (int num = 0; num <= numRound; num++)
        {
            int numRandom = Random.Range(0,5);
            //Debug.Log(numRandom);
            GameObject enemyInstance;

            if (PhotonNetwork.InRoom)
            { 
                enemyInstance = PhotonNetwork.Instantiate("MyZombie", spawnPoints[numRandom].transform.position, Quaternion.identity);
            }
            else
            { 
                enemyInstance = Instantiate(Resources.Load("MyZombie"),spawnPoints[numRandom].transform.position,Quaternion.identity) as GameObject;  
            }
            enemiesAlive++;
        }
    }

    public void RestartGame()
    {
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1; // Tornar a posar es temps
            SceneManager.LoadScene(2);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void BackToMainMenu()
    {
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1; // Tornar a posar es temps
        }
        Invoke("LoadMainMenuScene", 0.5f);
    }

    public void LoadMainMenuScene()
    {
        if (PhotonNetwork.InRoom)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    
    public void Pause()
    {
        pausePanel.SetActive(true);
        
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 0; // Aturar es temps
        }
        
        Cursor.lockState = CursorLockMode.None;

        isPaused = true;
    }
    
    public void Resume()
    {
        pausePanel.SetActive(false);
        
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 1; // Tornar a posar es temps
        }
        
        Cursor.lockState = CursorLockMode.Locked;

        isPaused = false;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        
        if (!PhotonNetwork.InRoom)
        {
            Time.timeScale = 0; // Aturar es temps
        }
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        isGameOver = true;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (photonView.IsMine)
        {
            if (changedProps["currentRound"] != null)
            {
                DisplayNextRound((int) changedProps["currentRound"]);
            }
        }
    }
}
