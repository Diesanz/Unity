using System;
using UnityEngine;

public class SnowballLauncher : MonoBehaviour
{
    public GameObject snowballPrefab;    // Prefab de la bola de nieve
    public Transform firePoint;          // Punto desde donde se lanza la bola de nieve
    public float throwForce = 15f;       // Fuerza de lanzamiento

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer del personaje
    }
    void Update()
    {
        // Verifica si se ha presionado la tecla F
        if (Input.GetKeyDown(KeyCode.E))
        {
            LaunchSnowball();
        }
    }

    void LaunchSnowball()
    {
        // Instancia la bola de nieve en el punto de lanzamiento
        GameObject snowball = Instantiate(snowballPrefab, firePoint.position, firePoint.rotation);

        // Añade una fuerza al Rigidbody2D para lanzar la bola de nieve
        Rigidbody2D rb2D = snowball.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            Debug.Log(spriteRenderer.name + " "+ spriteRenderer.flipX);
            // Lanza la bola en la dirección correcta (donde esta mirando)
            Vector2 launchDirection2 = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            rb2D.AddForce(launchDirection2 * throwForce, ForceMode2D.Impulse);
        }
    }

}
