using UnityEngine;

public class RangoEnemigo : MonoBehaviour
{
    /**
     * Referencia al componente Animator que controla las animaciones del enemigo.
     */
    public Animator ani;

    /**
     * Referencia al script que contiene los datos y comportamientos del enemigo.
     */
    public Enemies enemigo;

    /**
     * Este método se llama cuando un objeto entra en el rango de colisión del enemigo.
     * Si el objeto es el jugador, el enemigo deja de caminar y comienza a atacar.
     * Además, desactiva el collider para evitar que el enemigo siga detectando al jugador.
     * 
     * @param coll El collider del objeto que entra en contacto con el enemigo.
     */
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            // Detiene la animación de caminar y activa la de atacar.
            ani.SetBool("walk", false);
            ani.SetBool("attack", true);

            // Marca al enemigo como si estuviera atacando.
            enemigo.atacando = true;

            // Desactiva el BoxCollider2D para que el enemigo no siga detectando al jugador.
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
