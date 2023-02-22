using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    
    public GameObject playerCam; // Fa referència a la càmera del jugador FPS
    public float range = 100f; // Fins on volem que arribin els tirs

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButton(0))
       {
           Shoot();     
       } 
    }


    public void Shoot()
    {
        
        // Crea un rayo
        Ray ray = new Ray(playerCam.transform.position, transform.forward);
        
        // Realiza el Raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            // El rayo intersecta con algún objeto del escenario
            // Realiza las acciones correspondientes, como infligir daño al objeto o activar una animación de impacto
            Debug.Log("Hit object: " + hit.transform.name);
        }
        else
        {
            // El rayo no intersecta con ningún objeto del escenario
            // No es necesario realizar ninguna acción adicional
            Debug.Log("No hit");
        }
    }
}
