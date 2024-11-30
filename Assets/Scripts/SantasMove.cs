using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantasMove : MonoBehaviour
{   
    public float speed;         // Velocidad de movimiento
    public float jumpForce;     // Fuerza del salto
    public float fallMultiplier = 2.5f; // Multiplicador para caídas rápidas
    public float lowJumpMultiplier = 2f; // Multiplicador para saltos cortos

    private Vector2 input;
    private Rigidbody2D rb2D;   // Referencia al Rigidbody2D para controlar física
    private bool isJumpingAnimation = false;     // Indica si el jugador está saltando
    private bool isWalking;

    public SpriteRenderer spriteRenderer;
    public Animator animator;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Obtén la entrada del jugador
        input.x = Input.GetAxisRaw("Horizontal"); //Solo movimiento en el eje x

        // Cambiar velocidad del Rigidbody2D
        rb2D.velocity = new Vector2(input.x * speed, rb2D.velocity.y);
        // Invierte sprite renderer
        if (input.x != 0)
        {
            spriteRenderer.flipX = input.x < 0;
            isWalking = true;
        }    
        else{
            isWalking = false;
        }
        
        //Animacion movimiento
        animator.SetBool("Walk", isWalking);
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            
        // Control del salto
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround.isGround) 
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce); // Aplicar salto

        }

        // Mientras esté en el aire (descendiendo o ascendiendo)
        if (!CheckGround.isGround) 
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Walk", false);
        }
        else
        {
            // Si el personaje ha tocado el suelo, detener la animación de salto
            animator.SetBool("Jump", false);
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
