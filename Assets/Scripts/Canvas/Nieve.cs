using UnityEngine;

/**
 * Controla el movimiento vertical del fondo en un bucle infinito.
 * Cuando el fondo alcanza una posición específica (`endHeight`), 
 * se reinicia y aparece en otra posición definida (`resetHeight`).
 */
public class InfiniteScrolling : MonoBehaviour
{
    /** 
     * Velocidad a la que se mueve el fondo.
     */
    public float scrollSpeed = 2f;

    /** 
     * Altura a la que el fondo debe reiniciarse.
     */
    public float resetHeight;

    /** 
     * Altura donde el fondo termina su recorrido antes de reiniciarse.
     */
    public float endHeight;

    /** 
     * Posición inicial del fondo al iniciar el juego.
     */
    private Vector3 startPosition;

    /**
     * Inicializa la posición inicial del fondo.
     * Los fondos se colocan de forma que uno esté encima del otro al inicio, 
     * moviéndose simultáneamente para evitar superposiciones visibles.
     */
    void Start()
    {
        // Guarda la posición inicial del fondo.
        startPosition = transform.position;
    }

    /**
     * Actualiza la posición del fondo en cada frame, moviéndolo hacia abajo.
     * Cuando el fondo alcanza `endHeight`, se reinicia a `resetHeight`.
     */
    void Update()
    {
        // Mueve el fondo hacia abajo con la velocidad configurada.
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Comprueba si el fondo ha alcanzado la altura límite.
        if (transform.position.y <= endHeight)
        {
            // Reinicia la posición del fondo a la altura definida.
            transform.position = new Vector3(transform.position.x, resetHeight, transform.position.z);
        }
    }
}
