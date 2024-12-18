using UnityEngine;

/**
 * Clase para manejar los puntos de control en el juego.
 * Detecta cuando el jugador colisiona con la bandera (checkpoint) y ejecuta una acción correspondiente.
 */
public class CheckPoint : MonoBehaviour
{
    /**
     * Detecta cuando el jugador entra en contacto con el punto de control.
     * @param collider El objeto que entra en el área del trigger (generalmente el jugador).
     */
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verifica si el objeto que colisiona tiene la etiqueta "Player"
        if(collider.CompareTag("Player"))
        {
            // Llama al método ReachCheckpoint de PlayerRespawn para almacenar la posición del punto de control alcanzado
            collider.GetComponent<PlayerRespawn>().ReachedCheckpoint(transform.position.x, transform.position.y);

            // Activa el Animator para que se reproduzca la animación del checkpoint
            GetComponent<Animator>().enabled = true; // Se activa el animator para que la animación se reproduzca
        }
    }
}
