using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public Transform playerBody;

    public float mouseSensitivity = 500.0f;
    public float verticalRotationLimit = 80.0f;
    public float verticalRotation = 0;
    
    public PhotonView photonView;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Centrar el cursor.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; // Para asegurar que se bloquee despues de no verse

        // Establecer la rotación inicial del jugador.
        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }
        
        float hRotation = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float vRotation = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Aplicar rotación horizontal al jugador.
        //transform.Rotate(Vector3.up * hRotation);
        playerBody.Rotate(Vector3.up * hRotation);

        // Calcular la rotación vertical y limitarla al rango permitido.
        verticalRotation -= vRotation;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        // Aplicar la rotación vertical a la cámara.
        //transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }
}
