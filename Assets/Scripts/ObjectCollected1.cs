using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollected1 : MonoBehaviour
{
    private Animator animator;
    private bool isCollected = false; // Verifica si la fruta ya fue recolectada

    public string collectAnimationTrigger = "Recolect"; // Nombre del Trigger de la animación de explosión

    private void Start()
    {
        // Obtiene el componente Animator de la fruta
        animator = GetComponent<Animator>();
    }

    // Detectar cuando el jugador entra en contacto con la fruta
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si el objeto que colisiona es el jugador (asegúrate de que el jugador tiene el tag "Player")
        if (other.CompareTag("Player") && !isCollected)
        {
            isCollected = true;

            // Reproducir la animación de explosión
            if (animator != null)
            {
                animator.SetTrigger(collectAnimationTrigger); // Activa el Trigger de la animación de explosión
            }

            // Esperar a que termine la animación y luego destruir la fruta
            StartCoroutine(CollectFruit());
        }
    }

    private IEnumerator CollectFruit()
    {
        // Esperar el tiempo que dura la animación de explosión
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationDuration);

        // Después de la animación, destruir la fruta
        Destroy(gameObject); // O usa gameObject.SetActive(false) si prefieres desactivarla
    }

}
