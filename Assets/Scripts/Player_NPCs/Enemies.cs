using System.Collections;
using UnityEngine;


public class Enemies : MonoBehaviour
{
    /**
     * Rutina que controla el comportamiento del enemigo.
     */
    public int rutina;                 // Rutina de comportamiento del enemigo
    public float cronometro;           // Cronómetro para la asignación de rutina
    public Animator ani;               // Componente Animator del enemigo
    public int direccion;              // Dirección de movimiento (0: derecha, 1: izquierda)
    public float speed_walk = 2.0f;    // Velocidad de caminata
    public GameObject target;          // Objetivo (jugador)
    public bool atacando;              // Indica si el enemigo está atacando
    private Rigidbody2D rb;            // Componente Rigidbody2D para el movimiento físico
    public float rango_vision;         // Rango de visión del enemigo
    public float rango_ataque;         // Rango de ataque del enemigo
    public GameObject rango, hit;      // Referencias a los objetos de rango y ataque

    /**
     * Variables de límites de movimiento.
     */
    public float limiteIzquierdo;      // Límite izquierdo del movimiento del enemigo
    public float limiteDerecho;        // Límite derecho del movimiento del enemigo

    /**
     * Variables relacionadas con la vida del enemigo.
     */
    [SerializeField] private float vidaMAX;  // Vida máxima del enemigo
    public float vida;                  // Vida actual del enemigo
    private bool estaMuerto = false;    // Bandera que indica si el enemigo está muerto
    private float tiempoEntreGolpes = 1f;  // Tiempo de cooldown entre los golpes recibidos
    private float ultimoGolpe = 0f;     // Tiempo del último golpe recibido

    public AudioSource clip;            // Fuente de audio para la animación de muerte

    /**
     * Barra de salud.
     */
    [SerializeField] private HealthBar bar;

    /**
     * Función de inicio, establece valores iniciales.
     */
    void Start()
    {
        ani = GetComponent<Animator>();             // Obtiene el componente Animator
        target = GameObject.FindGameObjectWithTag("Player"); // Encuentra el jugador por su tag
        rb = GetComponent<Rigidbody2D>();           // Obtiene el componente Rigidbody2D
        vida = vidaMAX;                            // Establece la vida inicial
        //bar.UpdateHealthBar(vidaMAX, vida);      // Actualiza la barra de salud (comentado)
    }

    /**
     * Función Update, se llama una vez por frame.
     * Controla el comportamiento del enemigo durante el juego.
     */
    void Update()
    {
        Comportamiento();  // Llama al comportamiento del enemigo
    }

    /**
     * Función que define el comportamiento del enemigo.
     * Controla el movimiento, la interacción con el jugador y la animación.
     */
    public void Comportamiento()
    {
        if (ani.GetBool("death"))
        {
            return; // Sale si el enemigo está muerto
        }

        // Si el enemigo está fuera de rango de visión y no está atacando
        if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_vision && !atacando)
        {
            cronometro += Time.deltaTime;  // Aumenta el cronómetro con el tiempo transcurrido
            if (cronometro >= 4)           // Si pasa el tiempo, asigna una nueva rutina
            {
                rutina = Random.Range(0, 3); // Selecciona una rutina aleatoria
                cronometro = 0;               // Reinicia el cronómetro
            }

            // Comportamientos según la rutina asignada
            switch (rutina)
            {
                case 0:
                    ani.SetBool("walk", false); // No camina
                    break;
                case 1:
                    direccion = Random.Range(0, 2); // Selecciona una dirección aleatoria
                    ani.SetBool("walk", true);      // Empieza a caminar
                    rutina = 2;                     // Cambia a rutina 2
                    break;
                case 2:
                    Vector3 moveDir = Vector3.zero;       // Dirección de movimiento
                    Vector3 nuevaPosicion = transform.position; // Nueva posición

                    // Mueve al enemigo en función de la dirección
                    switch (direccion)
                    {
                        case 0: // Mover hacia la derecha
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            moveDir = transform.right * speed_walk * Time.deltaTime;
                            nuevaPosicion = transform.position + moveDir;
                            break;
                        case 1: // Mover hacia la izquierda
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            moveDir = transform.right * speed_walk * Time.deltaTime;
                            nuevaPosicion = transform.position + moveDir;
                            break;
                    }

                    // Verifica si la nueva posición está dentro de los límites
                    if (nuevaPosicion.x >= limiteIzquierdo && nuevaPosicion.x <= limiteDerecho)
                    {
                        rb.MovePosition(nuevaPosicion); // Mueve al enemigo
                        ani.SetBool("walk", true);       // Sigue caminando
                    }
                    else
                    {
                        ani.SetBool("walk", false);  // Si alcanza los límites, deja de caminar
                    }
                    break;
            }
        }
        else // Si el enemigo está cerca del jugador
        {
            if (Mathf.Abs(transform.position.x - target.transform.position.x) > rango_ataque && !atacando)
            {
                if (target.transform.position.x >= limiteIzquierdo && target.transform.position.x <= limiteDerecho)
                {
                    // Mueve hacia el jugador dependiendo de la posición
                    if (transform.position.x < target.transform.position.x)
                    {
                        rb.MovePosition((transform.right * (speed_walk * 2) * Time.deltaTime) + transform.position);
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        ani.SetBool("attack", false); // Deja de atacar
                    }
                    else
                    {
                        rb.MovePosition((transform.right * (speed_walk * 2) * Time.deltaTime) + transform.position);
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        ani.SetBool("attack", false); // Deja de atacar
                    }
                    ani.SetBool("walk", true); // Empieza a caminar hacia el jugador
                }
                else
                {
                    ani.SetBool("walk", false); // Si no está en rango, se detiene
                }
            }
            else
            {
                if (!atacando)
                {
                    // Ajusta la dirección de rotación dependiendo de la posición del jugador
                    if (transform.position.x < target.transform.position.x)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    ani.SetBool("walk", false); // Deja de caminar
                }
            }
        }
    }

    /**
     * Función que se llama al finalizar la animación de ataque.
     * Desactiva el ataque y activa el rango de ataque del enemigo.
     */
    public void Final_ani()
    {
        ani.SetBool("attack", false);  // Finaliza la animación de ataque
        atacando = false;              // Desactiva el estado de ataque
        rango.GetComponent<BoxCollider2D>().enabled = true;  // Activa el collider de rango
    }

    /**
     * Función que activa el collider del arma del enemigo.
     */
    public void ColliderWeaponTrue()
    {
        hit.GetComponent<BoxCollider2D>().enabled = true;  // Activa el collider del arma
    }

    /**
     * Función que desactiva el collider del arma del enemigo.
     */
    public void ColliderWeaponFalse()
    {
        hit.GetComponent<BoxCollider2D>().enabled = false;  // Desactiva el collider del arma
    }

    /**
     * Función que maneja la colisión del enemigo con otros objetos.
     * Detecta si el enemigo es golpeado por un proyectil y reduce su vida.
     */
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bola") && !estaMuerto) // Verifica si ya está muerto
        {
            // Control de cooldown para que no reciba daño repetidamente en un corto tiempo
            if (Time.time - ultimoGolpe < tiempoEntreGolpes) return;
            ultimoGolpe = Time.time;

            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
            {
                vida--; // Resta 1 a la vida
                bar.UpdateHealthBar(vidaMAX, vida);  // Actualiza la barra de salud
                if (vida > 0)
                {
                    ani.SetTrigger("hurt"); // Ejecuta la animación de daño
                }
                else
                {
                    // Si la vida es 0 o menos, activa la animación de muerte
                    ani.SetBool("death", true);
                    estaMuerto = true; // Marca al enemigo como muerto
                    StartCoroutine(DestruirEnemigo()); // Espera a que termine la animación y destruye el enemigo
                }

                // Dependiendo del estado de la animación actual, ajustamos las animaciones de movimiento o ataque
                if (ani.GetBool("walk"))
                {
                    ani.SetBool("walk", true); // Vuelve a caminar si está activo
                }
                else if (ani.GetBool("attack"))
                {
                    ani.SetBool("attack", true); // Vuelve a atacar si está activo
                }
            }
        }
    }

    /**
     * Corutina que destruye al enemigo después de un tiempo.
     * Desactiva al enemigo y lo destruye.
     */
    private IEnumerator DestruirEnemigo()
    {
        clip.Play(); // Reproduce el sonido de muerte
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length); // Espera duración animación
        Victoria victoriaScript = gameObject.GetComponent<Victoria>();
        if (victoriaScript != null)
        {
            victoriaScript.VictoriaEnemy(); // Llama al script si está presente
        }
        Destroy(gameObject); // Destruye el enemigo
    }

    /**
     * Función que recibe daño del jugador y actualiza su estado de salud.
     * */
    public void RecibirDaño(int daño)
    {
        vida -= daño;
        bar.UpdateHealthBar(vidaMAX, vida);
        if (vida <= 0)
        {
            ani.SetBool("death", true);  // Activa la animación de muerte
            estaMuerto = true;          // Marca como muerto
        }
    }
}
