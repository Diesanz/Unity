using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/**
 * Clase que gestiona la recolección de objetos en el juego.
 * Detecta la colisión con el jugador, reproduce una animación y elimina el objeto recolectado.
 */
public class ObjectCollected1 : MonoBehaviour
{
    /**
     * Referencia al componente Animator para manejar las animaciones del objeto.
     */
    private Animator animator;

    /**
     * Bandera que verifica si el objeto ya ha sido recolectado.
     */
    private bool isCollected = false;

    /**
     * Nombre del Trigger en el Animator que activa la animación de recolección.
     */
    public string collectAnimationTrigger = "Recolect";

    /**
     * Referencia al gestor de objetos en el juego.
     */
    private ObjectsManager objectsManager;

    /**
     * Fuente de audio para reproducir un efecto de sonido al recolectar el objeto.
     */
    public AudioSource clip;

    /**
     * Inicialización del script.
     * Se obtienen las referencias necesarias para el Animator y el ObjectsManager.
     */
    private void Start()
    {
        // Obtiene el componente Animator del objeto.
        animator = GetComponent<Animator>();

        // Encuentra y guarda una referencia al ObjectsManager en la escena.
        objectsManager = FindObjectOfType<ObjectsManager>();
    }

    /**
     * Detecta la colisión con el jugador y gestiona la recolección del objeto.
     *
     * @param other El Collider2D del objeto que entra en contacto con este.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica que el objeto colisionante sea el jugador y que el objeto no haya sido recolectado.
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Lógica específica para objetos con el tag "Rayo".
            if (gameObject.CompareTag("Rayo"))
            {
                // Si el jugador tiene menos de 3 vidas, se le otorga una más.
                if (other.GetComponent<PlayerRespawn>().life < 3)
                {
                    other.GetComponent<PlayerRespawn>().corazones[other.GetComponent<PlayerRespawn>().life].gameObject.SetActive(true);
                    other.GetComponent<PlayerRespawn>().life++;
                }
            }
            // Lógica específica para objetos con el tag "Caramelo".
            else if (gameObject.CompareTag("Caramelo"))
            {
                // Incrementa el número de bolas de nieve del jugador, hasta un máximo de 30.
                if (other.GetComponent<SnowballLauncher>().numberTotalBalls < 30)
                {
                    int incremento = (30 - other.GetComponent<SnowballLauncher>().numberTotalBalls > 5) ? 5 : (30 - other.GetComponent<SnowballLauncher>().numberTotalBalls);
                    other.GetComponent<SnowballLauncher>().numberTotalBalls += incremento;
                    other.GetComponent<SnowballLauncher>().numberBalls.text = other.GetComponent<SnowballLauncher>().numberTotalBalls.ToString();
                }
            }

            // Reproduce la animación de recolección.
            if (animator != null)
            {
                animator.SetTrigger(collectAnimationTrigger);
            }

            // Inicia la rutina para manejar la recolección.
            StartCoroutine(Collect());
        }
    }

    /**
     * Corrutina que espera a que termine la animación de recolección antes de destruir el objeto.
     */
    private IEnumerator Collect()
    {
        // Obtiene la duración de la animación actual en el Animator.
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;

        // Reproduce el clip de audio asociado.
        clip.Play();

        // Espera a que termine la animación.
        yield return new WaitForSeconds(animationDuration);

        // Destruye el objeto después de un breve retraso.
        Destroy(gameObject, 0.5f);
    }
}
