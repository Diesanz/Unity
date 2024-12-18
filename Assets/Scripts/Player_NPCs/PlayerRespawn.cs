using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class PlayerRespawn : MonoBehaviour
{
    /**
     * Arreglo de objetos que representan las vidas del jugador como corazones.
     */
    public GameObject[] corazones;

    /**
     * Número actual de vidas del jugador.
     */
    public int life;

    /**
     * Tiempo de invulnerabilidad que tiene el jugador después de recibir daño (en segundos).
     */
    public float invulnerabilityDuration = 0.2f;

    /**
     * Indica si el jugador es invulnerable o no.
     */
    public bool isInvulnerable = false;

    /**
     * Posición del checkpoint en el eje X.
     */
    private float checkpointPosX, checkpointPosY;

    /**
     * Referencia al componente Animator para controlar las animaciones del jugador.
     */
    public Animator animator;

    /**
     * Tiempo de espera antes de reiniciar la escena después de la muerte del jugador (en segundos).
     */
    public float respawnDelay = 0.5f;

    /**
     * Fuente de audio para el sonido de la muerte del jugador.
     */
    public AudioSource clip;

    /**
     * Fuente de audio para el sonido del daño recibido.
     */
    public AudioSource clipHurt;


    /**
     * Este método se llama al inicio del juego o cuando se instancia el objeto.
     * Inicializa el número de vidas y, si es necesario, restaura la posición del jugador al último checkpoint guardado.
     */
    void Start()
    {
        life = corazones.Length;

        // Si existen datos guardados para el checkpoint, restaura la posición del jugador
        if (PlayerPrefs.GetFloat("checkpointPosX") != 0)
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat("checkpointPosX"), PlayerPrefs.GetFloat("checkpointPosY"));
            
        }
    }

    /**
     * Este método verifica si el jugador ha perdido todas sus vidas.
     * Si el jugador tiene vida, se desactiva el correspondiente corazón.
     * También reproduce los sonidos y anima el jugador según el estado actual de vida.
     */
    public void CheckLive()
    {
        if (life < 1)
        {
            // Si el jugador se queda sin vidas, desactiva el primer corazón y maneja la muerte
            corazones[0].gameObject.SetActive(false);
            StartCoroutine(HandlePlayerDeath());
        }
        else if (life < 2)
        {
            // Si el jugador pierde la segunda vida, desactiva el segundo corazón
            corazones[1].gameObject.SetActive(false);
            clipHurt.Play();
            animator.Play("Hit");
        }
        else if (life < 3)
        {
            // Si el jugador pierde la tercera vida, desactiva el tercer corazón
            corazones[2].gameObject.SetActive(false);
            clipHurt.Play();
            animator.Play("Hit");
        }
    }

    /**
     * Este método guarda la posición del checkpoint actual en los datos del jugador.
     * @param x La coordenada X del checkpoint.
     * @param y La coordenada Y del checkpoint.
     */
    public void ReachedCheckpoint(float x, float y)
    {
        PlayerPrefs.SetFloat("checkpointPosX", x);
        PlayerPrefs.SetFloat("checkpointPosY", y);
    }

    /**
     * Este método se llama cuando el jugador recibe daño.
     * Si el jugador no está invulnerable, se reduce la cantidad de vidas y se maneja el daño.
     */
    public void PlayerDamaged()
    {
        if (!isInvulnerable) // El jugador solo recibe daño si no está invulnerable
        {
            life--;
            CheckLive();
            StartCoroutine(InvulnerabilityTimer());
        }
    }

    /**
     * Este método gestiona el tiempo de invulnerabilidad del jugador después de recibir daño.
     * Activa la invulnerabilidad durante un tiempo determinado y la desactiva después.
     */
    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true; // Activa la invulnerabilidad

        // Espera la duración de la animación de daño más un segundo adicional
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 1);

        isInvulnerable = false; // Desactiva la invulnerabilidad
    }

    /**
     * Este método maneja la muerte del jugador.
     * Reproduce la animación de muerte y el sonido correspondiente.
     * Después de que la animación termine, reinicia la escena actual.
     */
    private IEnumerator HandlePlayerDeath()
    {
        animator.Play("Dead");
        clip.Play();

        // Obtiene la duración de la animación de muerte
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Espera hasta que termine la animación
        yield return new WaitForSeconds(animationDuration);

        // Reinicia la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
