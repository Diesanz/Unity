using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantasMove : MonoBehaviour
{   
    public float speed;          // Velocidad de movimiento andar
    public float runspeed; // Velocidad de movimiento correr
    public float jumpForce = 8;     // Fuerza del salto
    public float doubleJumpForce = 6;
    private bool canDoubleJump;
    public float fallMultiplier = 2f; // Multiplicador para caídas rápidas
    public float lowJumpMultiplier = 2f; // Multiplicador para saltos cortos

    private Vector2 input;
    private Rigidbody2D rb2D;   // Referencia al Rigidbody2D para controlar física
    private bool isWalking;
    private bool isRunning;
    private bool isSlicing;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private float fallDelay = 0.5f; // Tiempo en segundos antes de activar "Falling"
    private float fallTimer;        // Temporizador para la caída
    private float doubleJumpTimer;
    public float minTimeJump = 0.2f;   // Tiempo mínimo antes de permitir el doble salto

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Obtén la entrada del jugador
        input.x = Input.GetAxisRaw("Horizontal"); //Solo movimiento en el eje x

        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float currentSpeed = isRunning ? runspeed : speed;
        // Cambiar velocidad del Rigidbody2D
        rb2D.velocity = new Vector2(input.x * currentSpeed, rb2D.velocity.y);
        // Invierte sprite renderer
        if (input.x != 0)
        {
            spriteRenderer.flipX = input.x < 0;
            isWalking = !isRunning;
            if (Input.GetKeyDown(KeyCode.F) && (isWalking || isRunning))
            {
                isWalking = false;
                isRunning = false;
                isSlicing = true;
            }
            else if (!Input.GetKey(KeyCode.F) || input.x == 0) // Detén el deslizamiento si se suelta F o no hay movimiento horizontal
            {
                isSlicing = false;
            }
            
        }    
        else{
            isWalking = false;
            isRunning = false;
            isSlicing = false;
        }
        
        //Animacion movimiento
        animator.SetBool("Walk", isWalking);
        animator.SetBool("Run", isRunning);
        animator.SetBool("Slice", isSlicing);
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if(CheckGround.isGround)
        {
            //reiniciar estado
            canDoubleJump = true;
            doubleJumpTimer = 0f;
        }
        else{
            //Incrementar tiempo desde el primer salto
            doubleJumpTimer += Time.deltaTime;
        }

        // Control del salto
        if (Input.GetKeyDown(KeyCode.Space) ) 
        {
            if (CheckGround.isGround) //si pulsamos espacio y estamos en suelo, se salta
            {
                canDoubleJump = true;
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce); // Aplicar salto
                doubleJumpTimer = 0f; // Reinicia el temporizador del doble salto
            }
            else if (canDoubleJump && doubleJumpTimer >= minTimeJump) // Doble salto
            {
                // Doble salto sólo si ha pasado el tiempo mínimo desde el primer salto
                canDoubleJump = false;
                rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpForce); // Aplicar doble salto   
                animator.SetBool("DoubleJump", true);
            }   

        }

        // Mientras esté en el aire (descendiendo o ascendiendo)
        if (!CheckGround.isGround) 
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            animator.SetBool("Slice", false);
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
