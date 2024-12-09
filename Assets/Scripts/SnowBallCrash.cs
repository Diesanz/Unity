using UnityEngine;
using System.Collections;

public class SnowBallCrash : MonoBehaviour
{
    private Animator animator;
    private bool isCrash = false; // Verifica si la colisión ya ocurrió

    void Start()
    {
        // Obtiene el componente Animator
        animator = GetComponent<Animator>();
        Debug.Log("a");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta colisión con el suelo
        if (collision.gameObject.CompareTag("Ground") && !isCrash)
        {
            isCrash = true;

            // Activa la animación de colisión
            if (animator != null)
            {
                animator.SetBool("Crash",true);
            }
            Debug.Log("Animación");

            // Destruye el objeto después de un breve tiempo para dejar que la animación se reproduzca
            StartCoroutine(DestroyAfterAnimation());
        }
    }
    private IEnumerator DestroyAfterAnimation()
    {
        // Espera el tiempo de la animación (ajusta el tiempo si es necesario)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        // Destruye la bola de nieve después de la animación
        Destroy(gameObject);
    }
}
