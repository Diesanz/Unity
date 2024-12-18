using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    /**
     * Referencia al componente PlatformEffector2D, que controla el comportamiento de la plataforma.
     */
    private PlatformEffector2D effector;

    /**
     * Tiempo de espera que se requiere antes de permitir que el jugador baje de la plataforma.
     */
    public float wait;

    /**
     * Tiempo acumulado para el conteo de espera.
     */
    private float waieted;

    /**
     * Este método se llama al iniciar el juego o cuando se instancia el objeto.
     * Obtiene la referencia al componente PlatformEffector2D adjunto a la plataforma.
     */
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    /**
     * Este método se llama una vez por cada fotograma durante la ejecución del juego.
     * Se encarga de verificar la entrada del jugador y gestionar el comportamiento de la plataforma.
     */
    void Update()
    {
        // Verifica si el jugador ha soltado la tecla de "abajo" (flecha hacia abajo o "S")
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp("s"))
        {
            waieted = wait;  // Restablece el tiempo de espera
        }

        // Verifica si el jugador está presionando la tecla de "abajo" (flecha hacia abajo o "S")
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s"))
        {
            // Si ha pasado el tiempo de espera, cambia el efecto de la plataforma
            if (waieted <= 0)
            {
                // Cambia el valor de rotationalOffset para hacer que la plataforma permita que el jugador caiga
                effector.rotationalOffset = 180f;
                waieted = wait;  // Restablece el tiempo de espera
            }
            else
            {
                // Si no ha pasado el tiempo de espera, disminuye el contador
                waieted -= Time.deltaTime;
            }
        }

        // Verifica si el jugador está presionando la tecla de "espacio"
        if (Input.GetKey(KeyCode.Space))
        {
            // Si se presiona la tecla de "espacio", resetea el valor de rotationalOffset
            effector.rotationalOffset = 0;
        }
    }
}
