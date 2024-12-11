using System;
using UnityEngine;
using UnityEngine.UI;

public class SnowballLauncher : MonoBehaviour
{
    public GameObject snowballPrefab;    // Prefab de la bola de nieve
    public Transform firePoint;          // Punto desde donde se lanza la bola de nieve
    public float throwForce = 15f;       // Fuerza de lanzamiento

    public Text totalBalls;
    public Text numberBalls;

    private SpriteRenderer spriteRenderer;
    private int numberTotalBalls;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer del personaje
        numberTotalBalls = 30;
        numberBalls.text = numberTotalBalls.ToString();
        totalBalls.text = numberTotalBalls.ToString();
    }
    void Update()
    {
        // Verifica si se ha presionado la tecla F
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(numberTotalBalls > 0)
            {   
                LaunchSnowball();
                numberTotalBalls -=1;
                numberBalls.text = numberTotalBalls.ToString();
            }
            else
            {
                numberBalls.color = Color.red;
            }
            
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
            // Lanza la bola en la dirección correcta (donde esta mirando)
            Vector2 launchDirection2 = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            // Añade un pequeño ángulo hacia arriba
            Vector2 adjustedLaunchDirection = (launchDirection2 + Vector2.up * 0.02f).normalized;

            // Aplica la fuerza
            rb2D.AddForce(adjustedLaunchDirection * throwForce, ForceMode2D.Impulse);
        }
    }

}
