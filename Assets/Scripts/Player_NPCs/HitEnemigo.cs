using UnityEngine;

public class HitEnemigo : MonoBehaviour
{
    /**
     * Referencia al componente Animator del objeto que contiene este script.
     */
    public Animator ani;

    /**
     * Referencia al objeto enemigo que es el que recibirá el daño.
     */
    public Enemies enemigo;
    
    /**
     * Este método se llama cuando otro collider entra en contacto con el collider de este objeto.
     * Si el objeto con el que colisiona tiene la etiqueta "Player", el jugador recibe daño.
     * 
     * @param coll El collider del objeto con el que se ha producido la colisión.
     */
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Verifica si el objeto con el que se ha colisionado es el jugador
        if(coll.CompareTag("Player"))
        {
            // Llama al método PlayerDamaged() del componente PlayerRespawn del jugador
            coll.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }
    }
}
