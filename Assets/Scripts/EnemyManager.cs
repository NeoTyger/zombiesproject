using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{

    public GameObject player;
    public Animator enemyAnimator;
    public GameManager gameManager;

    public float damage = 20f;
    
    // Salut de l'enemic
    public float health = 100f;

    public Slider healthBar;
    
    // Animacio i millora del xoc
    public bool playerInReach;
    public float attackDelayTimer;
    public float howMuchEarlierStartAttackAnimation = 1f;
    public float delayBetweenAttacks = 0.6f;

    public AudioSource enemyAudioSource;
    public AudioClip[] growlAudioClips;

    private GameObject[] playersInScene;

    // Start is called before the first frame update
    void Start()
    {
        // Aquest cop, no arrossegarem la variable GameObject del FPS
        // des de l'inspector, sinò que l'assginarem des del codi
        // En concret volem cercar al jugador principal!!
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
        gameManager = FindObjectOfType<GameManager>();
        
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    // Update is called once per frame
    void Update()
    {

        if (!enemyAudioSource.isPlaying)
        {
            enemyAudioSource.clip = growlAudioClips[Random.Range(0, growlAudioClips.Length)];
            enemyAudioSource.Play();
        }

        if (PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient)
        {
            return;
        }
        
        GetClosesPlayer();
        if (player != null)
        {
            // Accedim al component NavMeshComponent, el qual té un element que es destination de tipus Vector3
            // Li podem assignar la posició del jugador, que el tenim a la variable player gràcies al seu tranform
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        
            // D'aquesta forma ens aseguram de veure sa barra de vida encara que es ZOmbie estigui de costat
            healthBar.transform.LookAt(player.transform);
        }

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
            //Debug.Log("L'enemic m'ataca!!");
            //Debug.Log( "Health: " + PlayerManager.health + "ActualHealth: " + (PlayerManager.health - damage));
            //PlayerManager.Hit(damage);
            
            if(collision.gameObject == player)
            {
                //Debug.Log("L'enemic m'ataca!!");
                //player.GetComponent<PlayerManager>().hit(damage);
                playerInReach = true;

            }

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (playerInReach)
        {
            attackDelayTimer += Time.deltaTime;
            if (attackDelayTimer >= delayBetweenAttacks - howMuchEarlierStartAttackAnimation &&
                attackDelayTimer <= delayBetweenAttacks)
            {
                enemyAnimator.SetTrigger("isAttacking");
            }
            if(attackDelayTimer >= delayBetweenAttacks)
            {
                player.GetComponent<PlayerManager>().Hit(damage);
                attackDelayTimer = 0;
            }
        }

    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player)
        {
            playerInReach = false;
            attackDelayTimer = 0;
        }
    }


    public void Hit(float damage)
    {
        health -= damage;
        healthBar.value = health;
        
        if (health <= 0)
        {
            // Destrium a l'enemic quan la seva salut arriba a zero
            // feim referència a ell amb la variable gameObject, que fa referència al GO
            // que conté el componentn EnemyManager
            Destroy(gameObject);
            gameManager.enemiesAlive--;
            
            enemyAnimator.SetTrigger("IsDead");
            
            Destroy(gameObject,10f);
            Destroy(GetComponent<NavMeshAgent>());
            Destroy(GetComponent<EnemyManager>());
            Destroy(GetComponent<CapsuleCollider>());
        }
    }

    private void GetClosesPlayer()
    {
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject p in playersInScene)
        {
            if (p != null)
            {
                float distance = Vector3.Distance(p.transform.position, currentPosition);
                
                if (distance < minDistance)
                {
                    player = p;
                    minDistance = distance;
                }
            }
        }
    }
}
