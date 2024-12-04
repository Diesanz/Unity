using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantasMove : MonoBehaviour
{   
    public float speed;         // Velocidad de movimiento
    public float jumpForce = 8;     // Fuerza del salto
    public float doubleJumpForce = 6;
    private bool canDoubleJump;
    public float fallMultiplier = 2f; // Multiplicador para caídas rápidas
    public float lowJumpMultiplier = 2f; // Multiplicador para saltos cortos

    private Vector2 input;
    private Rigidbody2D rb2D;   // Referencia al Rigidbody2D para controlar física
    private bool isWalking;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private float fallDelay = 0.5f; // Tiempo en segundos antes de activar "Falling"
    private float fallTimer;        // Temporizador para la caída

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
        if (Input.GetKeyDown(KeyCode.Space) ) 
        {
            if (CheckGround.isGround) //si pulsamos espacio y estamos en suelo, se salta
            {
                canDoubleJump = true;
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce); // Aplicar salto
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    if(canDoubleJump)
                    {
                        canDoubleJump = false;
                        rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpForce); // Aplicar salto doble   
                        animator.SetBool("DoubleJump", true);
                    }
                }
                else
                {
                    rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
                }
            }

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
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Falling", false);
        }


        if (rb2D.velocity.y < 0)
        {
            // Iniciar el temporizador para la caída
            fallTimer += Time.deltaTime;
            
            if (fallTimer >= fallDelay)
            {
                canDoubleJump = false;
                animator.SetBool("Falling", true);
                
            }
        }
        else if (rb2D.velocity.y > 0)
        {
            // Resetea el temporizador si aún está subiendo
            fallTimer = 0;
            animator.SetBool("Falling", false);
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
