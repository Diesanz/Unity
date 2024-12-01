
using UnityEngine;

//Conocer cuando el player colisiona con la bandera
public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
        {
            collider.GetComponent<PlayerRespawn>().ReachedCheckpoint(transform.position.x, transform.position.y);
            GetComponent<Animator>().enabled = true; //se activa el animator para que directamente segun se pase se active la animacion
        }
    }
}
