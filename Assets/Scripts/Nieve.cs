using UnityEngine;

public class InfiniteBackgroundVertical : MonoBehaviour
{
    public float scrollSpeed = 2f; // Velocidad de movimiento del fondo
    public float resetPositionY1 = -1.7757f; // Posición Y en la que el primer fondo se reinicia
    public float resetPositionY2 = 7.55f; // Posición Y en la que el segundo fondo se reinicia
    public float startPositionY1 = 4.47f; // Posición Y inicial del primer fondo
    public float startPositionY2 = 15.3f; // Posición Y inicial del segundo fondo

    private Vector3 startPos1;
    private Vector3 startPos2;

    void Start()
    {
        // Establece la posición inicial para ambos fondos
        startPos1 = new Vector3(transform.position.x, startPositionY1, transform.position.z);
        startPos2 = new Vector3(transform.position.x, startPositionY2, transform.position.z);

        // Asegúrate de que los fondos empiecen en la posición correcta
        transform.position = startPos1; // Al principio se posiciona el primer fondo
    }

    void Update()
    {
        // Desplaza el fondo hacia abajo
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

        // Cuando el primer fondo sale de la pantalla, reposiciónalo
        if (transform.position.y <= resetPositionY1)
        {
            transform.position = startPos2; // Coloca el fondo al principio del segundo fondo
        }

        // Cuando el segundo fondo sale de la pantalla, reposiciónalo
        if (transform.position.y <= resetPositionY2 && transform.position == startPos2)
        {
            transform.position = startPos1; // Coloca el fondo al principio del primer fondo
        }
    }
}
