using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{

    public GameObject player;
    public Animator enemyAnimator;
    public GameManager gameManager;

    public float damage = 20f;
    
    // Salut de l'enemic
    public float health = 100f;

    // Start is called before the first frame update
    void Start()
    {
        // Aquest cop, no arrossegarem la variable GameObject del FPS
        // des de l'inspector, sinò que l'assginarem des del codi
        // En concret volem cercar al jugador principal!!
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Accedim al component NavMeshComponent, el qual té un element que es destination de tipus Vector3
        // Li podem assignar la posició del jugador, que el tenim a la variable player gràcies al seu tranform
        GetComponent<NavMeshAgent>().destination = player.transform.position;
        
        // En primer lloc hem d'accedir a la velocitat del Zombiem, des del component NavMeshAgent
        if (GetComponent<NavMeshAgent>().velocity.magnitude > 1)
        {
            enemyAnimator.SetBool("isRunning", true);
        }
        else
        {
            enemyAnimator.SetBool("isRunning", false);
        }
    }
    
    // Detectar la col·lisió
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject == player)
        {
            Debug.Log("L'enemic m'ataca!!");
            Debug.Log( "Health: " + PlayerManager.health + "ActualHealth: " + (PlayerManager.health - damage));
            PlayerManager.Hit(damage);
        }
    }
    
    public void Hit(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Destrium a l'enemic quan la seva salut arriba a zero
            // feim referència a ell amb la variable gameObject, que fa referència al GO
            // que conté el componentn EnemyManager
            Destroy(gameObject);
            gameManager.enemiesAlive--;
        }
    }

}
