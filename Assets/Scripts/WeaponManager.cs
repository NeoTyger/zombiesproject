using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    
    public GameObject playerCam; // Fa referència a la càmera del jugador FPS
    public float range = 100f; // Fins on volem que arribin els tirs
    
    public float damage = 10f;

    public Animator playerAnimator;
    
    // Referència per a gestionar el sistema de particules
    public ParticleSystem flashParticleSystem;
    public GameObject bloodParticleSystem;
    
    private AudioSource weaponAudio;
    
    public PhotonView photonView;

    public GameManager gameManager;

    public AudioClip shootClip;


    // Start is called before the first frame update
    void Start()
    {
        weaponAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }
        
        if (!gameManager.isPaused && !gameManager.isGameOver)
        {
            if (playerAnimator.GetBool("isShooting"))
            {
                playerAnimator.SetBool("isShooting", false);
            }

            if (Input.GetMouseButton(0))
            {
                Shoot();
            }
        }
    }

    private void Shoot()
   {
       if (PhotonNetwork.InRoom)
       {
           photonView.RPC("WeaponShootSFX", RpcTarget.All, photonView.ViewID);
       }
       else
       {
           ShootVFX(photonView.ViewID);
       }
       
       playerAnimator.SetBool("isShooting", true);

       RaycastHit hit;
       if (Physics.Raycast(playerCam.transform.position, transform.forward, out hit, range))
       {
           //Debug.Log("Tocat!");
           // Si no hem ferit a un Zombie, la component EnemyManager valdrà null, però sinò prendrà el valor de la component del Zombie que hem ferit.
           EnemyManager enemyManager = hit.transform.GetComponent<EnemyManager>();
           if(enemyManager != null)
           {
               // generam una instància del particle system, en el punt on hem ferit al Zombie,
               // i fent que l'animació sempre estigui rotada en direcció al tret
               GameObject particleInstance = Instantiate(bloodParticleSystem, hit.point, Quaternion.LookRotation(hit.normal));
               // Feim que la instància sigui filla del Zombie al qual hem ferit
               particleInstance.transform.parent = hit.transform;
               // Recordau que aquesta animació te seleccionat per Stop Action: "Destroy" ja que sinó es crearien infinites instàncies

               enemyManager.Hit(damage);
           }
       }
   }

    public void ShootVFX(int viewID)
    {
        if (photonView.ViewID == viewID)
        {
            flashParticleSystem.Play();
            weaponAudio.PlayOneShot(shootClip, 0.75f);
        }
    }

}
