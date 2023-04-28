using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager sharedInstance;
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

    private void OnEnable()
    {
        // Nos suscribimos al evento
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        // Anulamos suscripción
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // Anulamos suscripción
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-2f, 2f), 2, Random.Range(-2f, 2f));

        if (PhotonNetwork.InRoom)
        {
            // Estamos Online
            PhotonNetwork.Instantiate("First_Person_Player_With_Camera", spawnPosition, Quaternion.identity);
        }
        else
        {
            // Singleplayer
            Instantiate(Resources.Load("First_Person_Player_With_Camera"), spawnPosition, Quaternion.identity);
        }
    }
}
