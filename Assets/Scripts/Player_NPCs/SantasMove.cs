using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantasMove : MonoBehaviour
{
    // Variables de movimiento
    public float speed;  // Velocidad de movimiento andar
    public float runspeed; // Velocidad de movimiento correr
    public float jumpForce = 8; // Fuerza del salto
    public float doubleJumpForce = 6; // Fuerza para el doble salto
    private bool canDoubleJump; // Indica si el jugador puede realizar un doble salto
    public float fallMultiplier = 2f; // Multiplicador para caídas rápidas
    public float lowJumpMultiplier = 2f; // Multiplicador para saltos cortos

    // Variables de entrada y físicas
    private Vector2 input; // Entrada del jugador para el movimiento
    private Rigidbody2D rb2D; // Referencia al Rigidbody2D para controlar la física del movimiento
    private bool isWalking = false; // Indica si el jugador está caminando
    private bool isRunning; // Indica si el jugador está corriendo
    private bool isSlicing = false; // Indica si el jugador está realizando el deslizamiento

    // Variables de animación y sprites
    public SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer para el control de dirección del personaje
    public Animator animator; // Referencia al Animator para controlar las animaciones
    private float fallDelay = 0.5f; // Tiempo en segundos antes de activar la animación de caída
    private float fallTimer; // Temporizador para la animación de caída
    private float doubleJumpTimer; // Temporizador para el doble salto

    // Variables de colisión
    public float minTimeJump = 0.2f; // Tiempo mínimo antes de permitir el doble salto
    private BoxCollider2D boxCollider; // Referencia al BoxCollider2D para la detección de colisiones
    public BoxCollider2D boxColliderGround; // Referencia al BoxCollider2D que controla la colisión con el suelo
    private Vector2 originalOffset; // Offset original del collider
    private Vector2 originalOffsetGround; // Offset original del collider de la comprobación del suelo
    private Vector2 originalSize; // Tamaño original del collider
    private bool isAir = false; // Indica si el jugador está en el aire (saltando o cayendo)

    private void Awake()
    {
        // Inicialización de componentes
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        originalSize = boxCollider.size;
        originalOffset = boxCollider.offset;
        originalOffsetGround = boxColliderGround.offset;
    }

    private void Update()
    {
        // Verificación de invulnerabilidad
        if (GetComponent<PlayerRespawn>().isInvulnerable)
        {
            rb2D.velocity = new Vector2(0, rb2D.velocity.y); // Detener movimiento horizontal
            animator.SetBool("Walk", false);
            animator.SetBool("Run", false);
            return; // Salir del Update
        }

        // Obtención de la entrada del jugador (movimiento en el eje X)
        input.x = Input.GetAxisRaw("Horizontal");

        // Comprobación si el jugador está corriendo
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Definir la velocidad actual dependiendo de si el jugador está corriendo o caminando
        float currentSpeed = isRunning ? runspeed : speed;
        rb2D.velocity = new Vector2(input.x * currentSpeed, rb2D.velocity.y); // Aplicar la velocidad al Rigidbody2D

        // Invertir el sprite dependiendo de la dirección
        if (input.x != 0)
        {
            spriteRenderer.flipX = input.x < 0;
            isWalking = !isRunning;

            if (!isSlicing)
            {
                AdjustCollider(input.x < 0); // Ajustar el collider si no se está deslizando
            }

            if (!isAir)
            {
                // Deslizamiento
                if (Input.GetKeyDown(KeyCode.F) && (isWalking || isRunning))
                {
                    StartCoroutine(AjustColliderSlice(true));
                }
                if (Input.GetKeyUp(KeyCode.F) || input.x == 0) // Detener deslizamiento si se suelta la tecla o no hay movimiento
                {
                    if (isSlicing)
                    {
                        StartCoroutine(AjustColliderSlice(false));
                    }
                }
            }
        }
        else
        {
            isWalking = false;
            isRunning = false;
            if (isSlicing)
            {
                StartCoroutine(AjustColliderSlice(false));
            }
        }

        // Animación de movimiento
        animator.SetBool("Walk", isWalking);
        animator.SetBool("Run", isRunning);

        // Comprobación de daño si el jugador cae por debajo de una altura determinada
        if (transform.position.y < -1.13f)
        {
            transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }

        // Reseteo del doble salto cuando se está en el suelo
        if (CheckGround.isGround)
        {
            canDoubleJump = true;
            doubleJumpTimer = 0f;
        }
        else
        {
            doubleJumpTimer += Time.deltaTime; // Incremento del temporizador de doble salto
        }

        // Control del salto
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckGround.isGround)
            {
                canDoubleJump = true;
                rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce); // Salto simple
                doubleJumpTimer = 0f;
            }
            else if (canDoubleJump && doubleJumpTimer >= minTimeJump)
            {
                // Doble salto
                canDoubleJump = false;
                rb2D.velocity = new Vector2(rb2D.velocity.x, doubleJumpForce); // Aplicar doble salto
                animator.SetBool("DoubleJump", true);
            }
        }

        // Manejo del estado de estar en el aire (saltando o cayendo)
        if (!CheckGround.isGround)
        {
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
            animator.SetBool("Jump", false);
            animator.SetBool("DoubleJump", false);
            animator.SetBool("Falling", false);
            isAir = false;
        }

        // Manejo de la caída
        if (rb2D.velocity.y < 0)
        {
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
            fallTimer = 0;
            animator.SetBool("Falling", false);
            if (!isSlicing)
            {
                AdjustCollider(spriteRenderer.flipX);
            }
        }

        // Caída rápida
        if (rb2D.velocity.y < 0)
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        // Salto más corto si no se mantiene presionada la tecla de salto
        else if (rb2D.velocity.y > 0 && Input.GetKey(KeyCode.Space))
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void AdjustCollider(bool isFlipped)
    {
        // Ajustar el collider dependiendo de si el sprite está volteado
        if (isFlipped)
        {
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
        // Esperar hasta que la animación comience
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null;
        }

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.1f)
        {
            yield return null;
        }

        // Ajustar el colisionador después de que la animación haya comenzado
        Vector2 newOffset = new Vector2(boxCollider.offset.x, boxCollider.offset.y / 0.08f);
        Vector2 newSize = new Vector2(boxCollider.size.x, boxCollider.size.y / 3.5f);
        boxCollider.offset = newOffset;
        boxCollider.size = newSize;
    }
}
