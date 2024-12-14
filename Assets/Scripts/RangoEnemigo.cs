using UnityEngine;

public class RangoEnemigo: MonoBehaviour
{
    public Animator ani;
    public Enemies enemigo;
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            ani.SetBool("walk", false);
            ani.SetBool("attack", true);
            enemigo.atacando = true;
            GetComponent<BoxCollider2D>().enabled = false;
        }

    }
}