using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantasMove : MonoBehaviour
{   
    public float speed;         // Velocidad de movimiento
    public float jumpForce;     // Fuerza del salto
    public bool jumpForcex2 = false;  // Activar doble salto (opcional)
    public float fallMultiplier = 2.5f; // Multiplicador para caídas rápidas
    public float lowJumpMultiplier = 2f; // Multiplicador para saltos cortos

    private Vector2 input;
    private Rigidbody2D rb2D;    // Referencia al Rigidbody2D para controlar física

    

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Obtén la entrada del jugador
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // Permite movimiento en una dirección a la vez
        if (input.x != 0) input.y = 0;

        // Mueve el jugador si hay entrada
        if (input != Vector2.zero)
        {
            // Calcula la nueva posición y mueve al jugador
            Vector3 targetPos = transform.position + new Vector3(input.x, input.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        
        // Salto
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround.isGround)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce); // Cambiar solo el eje Y para saltar
        }

        // Manejo de la caída y salto prolongado
        if (rb2D.velocity.y < 0)
        {
            // Caída rápida
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // Salto más corto si no se mantiene presionada la tecla de salto
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
