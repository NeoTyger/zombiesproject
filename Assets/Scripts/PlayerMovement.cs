using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private CharacterController _controller;

    // Velocidad de movimiento para el jugador
    public float speed;
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    
    // Variables para la garvedad
    private Vector3 velocity;
    public float gravity = -9.81f;
    
    // GroundCheck
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f; //Umbral de distancia del suelo
    public LayerMask groundMask;
    
    // Jump
    public float jumpHeight = 2f;

    private void Start()
    {
        _controller = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Obtiene el movimiento horizontal y vertical
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calcula la dirección del movimiento
        //Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movement = transform.right * horizontalInput + transform.forward * verticalInput;
        _controller.Move(movement * speed * Time.deltaTime);

        // Gravedad
        // Fórmula: velocidad = aceleración * tiempo ^ 2
        velocity.y += gravity * Time.deltaTime;
        _controller.Move(velocity * Time.deltaTime);
        
        // Mira si estoy tocando el suelo
        isGrounded = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetButton("Fire3") && isGrounded)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }
    }
}
