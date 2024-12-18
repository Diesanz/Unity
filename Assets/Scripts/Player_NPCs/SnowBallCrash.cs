using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SnowBallCrash : MonoBehaviour
{
    /**
     * Referencia al componente Animator que controla las animaciones de la bola de nieve.
     */
    private Animator animator;

    /**
     * Indica si la colisión ya ha ocurrido para evitar que se active varias veces.
     */
    private bool isCrash = false;

    /**
     * Fuente de audio para reproducir el sonido de la colisión.
     */
    public AudioSource clip;

    /**
     * Este método se llama al iniciar el juego.
     * Se encarga de obtener el componente Animator del objeto para controlar las animaciones.
     */
    void Start()
    {
        // Obtiene el componente Animator para controlar las animaciones de la bola de nieve.
        animator = GetComponent<Animator>();
    }

    /**
     * Este método se llama cuando la bola de nieve colisiona con otro objeto.
     * Si la bola de nieve colisiona con el suelo o un enemigo, activa la animación de colisión y reproduce el sonido.
     * Además, destruye el objeto después de un breve tiempo.
     * 
     * @param collision La colisión detectada con otro objeto.
     */
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si la bola de nieve colisiona con el suelo o un enemigo y si no ha ocurrido una colisión previamente.
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemie")) && !isCrash)
        {
            // Marca que la colisión ha ocurrido para evitar repetirla.
            isCrash = true;

            // Si el componente Animator está presente, activa la animación de colisión.
            if (animator != null)
            {
                // Activa el parámetro de la animación para reproducir la animación de colisión.
                animator.SetBool("Crash", true);

                // Reproduce el sonido de la colisión.
                clip.Play();
            }

            // Imprime en la consola que la animación de colisión se ha activado.
            Debug.Log("Animación");

            // Llama a un Coroutine para destruir el objeto después de que se reproduzca la animación.
            StartCoroutine(DestroyAfterAnimation());
        }
    }

    /**
     * Coroutine que espera el tiempo necesario para que se complete la animación de colisión.
     * Después de la animación, destruye el objeto de la bola de nieve.
     */
    private IEnumerator DestroyAfterAnimation()
    {
        // Espera el tiempo necesario para que se reproduzca la animación.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destruye el objeto de la bola de nieve después de que se complete la animación.
        Destroy(gameObject);
    }
}
