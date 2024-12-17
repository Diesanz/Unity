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
    private bool isWalking = false;
    private bool isRunning;
    private bool isSlicing = false;

    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private float fallDelay = 0.5f; // Tiempo en segundos antes de activar "Falling"
    private float fallTimer;        // Temporizador para la caída
    private float doubleJumpTimer;
    private bool isJumping = false;
    public float minTimeJump = 0.2f;   // Tiempo mínimo antes de permitir el doble salto
    private BoxCollider2D boxCollider;
    public BoxCollider2D boxColliderGround;
    private Vector2 originalOffset; // Offset original del collider
    private Vector2 originalOffsetGround; // Offset del collider del CheckGround
    private Vector2 originalSize; // Tamaño original del collider
    private bool isAir = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;
        originalOffsetGround = boxColliderGround.offset;
    }

    private void Update()
    {
        if (GetComponent<PlayerRespawn>().isInvulnerable)
        {
            Debug.Log("Boqq");
            rb2D.velocity = new Vector2(0, rb2D.velocity.y); // Detener movimiento horizontal
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            return; // Salir del Update
        }
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

            if(!isSlicing)
            {
                AdjustCollider(input.x<0);
            }
            
            
            if(!isAir)
            {
                
                if (Input.GetKeyDown(KeyCode.F) && (isWalking || isRunning))
                {
                    StartCoroutine(AjustColliderSlice(true));
                }
                if (Input.GetKeyUp(KeyCode.F) || input.x == 0) // Detén el deslizamiento si se suelta F o no hay movimiento horizontal
                {
                    if(isSlicing)
                    {
                        Debug.Log("SlicingStop");
                        StartCoroutine(AjustColliderSlice(false));
                    }
                    
                }
            }

            
        }    
        else
        {
            isWalking = false;
            isRunning = false;
            if(isSlicing)
            {
                Debug.Log("SlicingStop");
                StartCoroutine(AjustColliderSlice(false));
            }

           /* if(isAir)
            {
                 Debug.LogWarning("Aire2"+ isAir + ", Flix" + (spriteRenderer.flipX));
                 AdjustCollider(spriteRenderer.flipX);
            }*/
        }
        
        
        //Animacion movimiento
        animator.SetBool("Walk", isWalking);
        animator.SetBool("Run", isRunning);
        
        if (transform.position.y < -1.13f)
        {
            // Llama al método para manejar el daño al jugador
            transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }

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
                //AdjustCollider(input.x<0);
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
            StartCoroutine(AjustColliderSlice(false));
            
            animator.SetBool("Jump", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            isRunning = false;
            isWalking = false;
            isSlicing = false;
            isAir = true;
                
        }
        else
        {
            // Si el personaje ha tocado el suelo, detener la animación de salto
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Falling", false);
            isAir = false;
        }


        if (rb2D.velocity.y < 0)
        {
            // Iniciar el temporizador para la caída
            fallTimer += Time.deltaTime;
            
            if (fallTimer >= fallDelay)
            {
                canDoubleJump = false;
                animator.SetBool("Falling", true);
                AdjustCollider(spriteRenderer.flipX);
                
            }
        }
        else if (rb2D.velocity.y > 0)
        {
            
            // Resetea el temporizador si aún está subiendo
            fallTimer = 0;
            animator.SetBool("Falling", false);
            if(!isSlicing)
            {
                AdjustCollider(spriteRenderer.flipX);
            }
            
        }
       

        // Manejo de la caída y salto prolongado
        if (rb2D.velocity.y < 0)
        {
            Debug.Log("Falling");
            
            // Caída rápida
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb2D.velocity.y > 0 && Input.GetKey(KeyCode.Space))
        {
            
            // Salto más corto si no se mantiene presionada la tecla de salto
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    void AdjustCollider(bool isFlipped)
    {
        // Si el sprite está volteado (flipX es true), ajustamos el offset
        if (isFlipped)
        {
            Debug.Log("Position change");
            boxCollider.offset = new Vector2(-originalOffset.x, originalOffset.y);
            boxColliderGround.offset = new Vector2(-originalOffsetGround.x, originalOffsetGround.y);
            
        }
        else
        {
            boxCollider.offset = originalOffset;
            boxColliderGround.offset = originalOffsetGround;
        }
    }
    IEnumerator AjustColliderSlice(bool isSlice)
    {
        if (isSlice)
        {
            isSlicing = true;
            animator.SetBool("Slice", isSlicing);

            // Iniciar la corrutina para esperar el inicio de la animación
            StartCoroutine(WaitForAnimationToStart("Slide"));
            
            isWalking = false;
            isRunning = false;
                        
        }
        else
        {
            isSlicing = false;
            animator.SetBool("Slice", isSlicing);
            boxCollider.offset = originalOffset;
            boxCollider.size = originalSize;
            
        }
        yield return new WaitForSeconds(0.1f); 
        
    }
    private IEnumerator WaitForAnimationToStart(string animationName)
    {
        // Esperar hasta que el Animator cambie al estado deseado
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null; // Espera un frame
        }

        Debug.Log("La animación 'Slide' ha comenzado");

        // Esperar hasta un punto específico de la animación (opcional)
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.1f) // Ajusta según el momento que necesites
        {
            yield return null; // Espera un frame
        }

        Debug.Log("Tiempo suficiente en 'Slide'. Ajustando colisionador...");

        // Ajustar el colisionador aquí
        Vector2 newOffset = new Vector2(boxCollider.offset.x, boxCollider.offset.y / 0.08f);
        Vector2 newSize = new Vector2(boxCollider.size.x, boxCollider.size.y / 3.5f);

        boxCollider.offset = newOffset;
        boxCollider.size = newSize;
    }


}
