using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class SnowBallCrash : MonoBehaviour
{
    private Animator animator;
    private bool isCrash = false; // Verifica si la colisión ya ocurrió
    public AudioSource clip;

    void Start()
    {
        // Obtiene el componente Animator
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Detecta colisión con el suelo
        if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Enemie")) && !isCrash)
        {
            isCrash = true;

            // Activa la animación de colisión
            if (animator != null)
            {
                animator.SetBool("Crash",true);
                clip.Play();
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
