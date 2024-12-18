using System;
using UnityEngine;
using UnityEngine.UI;

public class SnowballLauncher : MonoBehaviour
{
    /**
     * Prefab de la bola de nieve que se lanzará.
     */
    public GameObject snowballPrefab;

    /**
     * Punto de lanzamiento desde donde se generará la bola de nieve.
     */
    public Transform firePoint;

    /**
     * Fuerza con la que se lanzará la bola de nieve.
     */
    public float throwForce = 15f;

    /**
     * Tiempo de espera entre lanzamientos para evitar lanzamientos demasiado rápidos.
     */
    public float throwCooldown = 0.5f;

    /**
     * Texto de la UI que muestra la cantidad total de bolas de nieve disponibles.
     */
    public Text totalBalls;

    /**
     * Texto de la UI que muestra la cantidad actual de bolas de nieve.
     */
    public Text numberBalls;

    /**
     * Componente SpriteRenderer del personaje, usado para controlar la orientación.
     */
    private SpriteRenderer spriteRenderer;

    /**
     * El tiempo del último lanzamiento de bola de nieve.
     */
    private float lastThrowTime;

    /**
     * Número total de bolas de nieve disponibles para lanzar.
     */
    public int numberTotalBalls;

    private int realTotalBalls = 30;

    /**
     * Este método se llama al iniciar el juego.
     * Se encarga de inicializar las variables y de mostrar la cantidad inicial de bolas de nieve en la UI.
     */
    void Start()
    {
        // Inicializa el SpriteRenderer para controlar la orientación del personaje.
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Establece el número inicial de bolas de nieve.
        if (PlayerPrefs.HasKey("Bolas"))
        {
            // Si existe, cargamos el valor guardado.
            numberTotalBalls = PlayerPrefs.GetInt("Bolas");
            numberBalls.text = numberTotalBalls.ToString();
            
        }
        else
        {
            // Si no existe, asignamos el tiempo límite predeterminado.
            numberTotalBalls = 30;
            numberBalls.text = numberTotalBalls.ToString();
           
        }
        totalBalls.text = realTotalBalls.ToString();

        // Actualiza la interfaz de usuario con el número inicial de bolas de nieve.
        

        // Permite el primer lanzamiento inmediatamente al comenzar.
        lastThrowTime = -throwCooldown;
    }

    /**
     * Este método se llama en cada frame.
     * Detecta la entrada del jugador y maneja el lanzamiento de bolas de nieve.
     */
    void Update()
    {
       numberBalls.color = (numberTotalBalls > 0) ? Color.white : Color.red;
        // Verifica si el jugador presiona la tecla E para lanzar una bola de nieve.
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Comprueba si ha pasado el tiempo suficiente desde el último lanzamiento.
            if (Time.time - lastThrowTime >= throwCooldown)
            {
                // Verifica si todavía hay bolas de nieve disponibles para lanzar.
                if (numberTotalBalls > 0)
                {
                    // Lanza la bola de nieve.
                    LaunchSnowball();
                    // Reduce el número de bolas de nieve disponibles.
                    numberTotalBalls -= 1;
                    PlayerPrefs.SetInt("Bolas", numberTotalBalls);
                    // Actualiza la interfaz de usuario con la cantidad restante de bolas.
                    numberBalls.text = numberTotalBalls.ToString();
                    // Actualiza el tiempo del último lanzamiento.
                    lastThrowTime = Time.time;
                    numberBalls.color = Color.white;
                }
                else if (numberTotalBalls <= 0)
                {
                    // Si no hay bolas de nieve, cambia el color del texto a rojo.
                    numberBalls.color = Color.red;
                }
            }
        }
    }

    /**
     * Método que maneja el lanzamiento de la bola de nieve.
     * Instancia la bola de nieve en la escena y aplica una fuerza para lanzarla.
     */
    void LaunchSnowball()
    {
        // Instancia una nueva bola de nieve en el punto de lanzamiento.
        GameObject snowball = Instantiate(snowballPrefab, firePoint.position, firePoint.rotation);

        // Obtiene el componente Rigidbody2D de la bola de nieve para aplicar física.
        Rigidbody2D rb2D = snowball.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            // Define la dirección de lanzamiento dependiendo de si el personaje está mirando hacia la izquierda o derecha.
            Vector2 launchDirection2 = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            // Ajusta la dirección de lanzamiento para incluir un pequeño ángulo hacia arriba.
            Vector2 adjustedLaunchDirection = (launchDirection2 + Vector2.up * 0.02f).normalized;

            // Aplica la fuerza de lanzamiento a la bola de nieve.
            rb2D.AddForce(adjustedLaunchDirection * throwForce, ForceMode2D.Impulse);
        }
    }
}