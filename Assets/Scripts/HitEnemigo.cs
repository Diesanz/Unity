using UnityEngine;

public class HitEnemigo: MonoBehaviour
{
    public Animator ani;
    public Enemies enemigo;
    
    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            coll.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }

    }
}