using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 500.0f;
    public float verticalRotationLimit = 80.0f;
    public float verticalRotation = 0;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float hRotation = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float vRotation = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Aplicar rotación horizontal al jugador.
        transform.Rotate(Vector3.up * hRotation);

        // Calcular la rotación vertical y limitarla al rango permitido.
        verticalRotation -= vRotation;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);

        // Aplicar la rotación vertical a la cámara.
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0);
    }
}
