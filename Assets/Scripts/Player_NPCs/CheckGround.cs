
using UnityEngine;

/**
 * Clase para verificar si un objeto está en contacto con el suelo.
 * Utiliza un `Collider2D` en el objeto para detectar colisiones con el suelo.
 */
public class CheckGround : MonoBehaviour
{
    // Variable estática para indicar si el objeto está tocando el suelo
    public static bool isGround;

    /**
     * Detecta cuando el objeto entra en contacto con otro objeto etiquetado como "Ground".
     * @param collision El objeto con el que se produce la colisión.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprueba si el objeto colisionado tiene la etiqueta "Ground"
        if (collision.CompareTag("Ground"))
        {
            isGround = true; // Establece que el objeto está en el suelo
        }
    }

    /**
     * Detecta cuando el objeto deja de estar en contacto con otro objeto etiquetado como "Ground".
     * @param collision El objeto con el que se produce la colisión.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Comprueba si el objeto colisionado tiene la etiqueta "Ground"
        if (collision.CompareTag("Ground"))
        {
            isGround = false; // Establece que el objeto ya no está en el suelo
        }
    }
}
