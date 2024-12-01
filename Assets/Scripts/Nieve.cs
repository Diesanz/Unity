using UnityEngine;
//Control del movimiento vertical del fondo
//Cuando un fondo llega al final endHeight se resetea y aparece en resetHeight
public class InfiniteScrolling : MonoBehaviour
{
    public float scrollSpeed = 2f; // Velocidad a la que se mueve el fondo
    public float resetHeight; // La altura donde el fondo debe reiniciarse 
    public float endHeight; // La altura donde el fondo termina su recorrido 

    private Vector3 startPosition; // Posición inicial del fondo

    /*
    *Para no superponer fondos al principio se colocan encima unos de otros
    *Ambos se mueven a la vez
    */
    void Start()
    {
        // Guardamos la posición inicial del fondo
        startPosition = transform.position;
    }

    void Update()
    {
        // Mueve el fondo hacia abajo
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // Si el fondo alcanza el final de su recorrido, se reinicia
        if (transform.position.y <= endHeight)
        {
            transform.position = new Vector3(transform.position.x, resetHeight, transform.position.z);
        }
    }
}
