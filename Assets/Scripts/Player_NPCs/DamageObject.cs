using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Clase para manejar el daño infligido a objetos (como el jugador) cuando colisionan.
 * En este caso, cuando un objeto con esta clase colisiona con el jugador, se ejecuta un efecto de daño.
 */
public class DamageObject : MonoBehaviour
{
    /**
     * Detecta cuando otro objeto colisiona con este objeto.
     * Si el objeto colisionado es el jugador, se ejecuta el daño.
     * @param collision La colisión detectada con el jugador.
     */
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si el objeto que colisiona tiene la etiqueta "Player"
        if (collision.transform.CompareTag("Player"))
        {
            // Llama al método PlayerDamaged del componente PlayerRespawn
            collision.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }
    }
}
