using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referencia al personaje
    public Vector3 offset;   // Desplazamiento de la cámara
    public float smoothSpeed = 4; // Velocidad de suavizado

    private Camera cam;

    // Limites de la cámara
    private float minX, maxX, minY, maxY;

    void Start()
    {
        cam = Camera.main;

        // Establece los límites de la cámara en base a los valores proporcionados
        minX = 17.44f;   // Límite izquierdo (posición mínima X)
        maxX = 148.65f;  // Límite derecho (posición máxima X)
        minY = 2.604295f; // Límite inferior (posición mínima Y)
        maxY = 6.71f;     // Límite superior (posición máxima Y)
    }

    void LateUpdate()
    {
        // Obtener la posición deseada de la cámara, ajustada por el offset
        Vector3 desiredPosition = target.position + offset;

        // Aplicar límites a la posición de la cámara
        float clampedX = Mathf.Clamp(desiredPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // Si no quieres limitar el movimiento en el eje Z (o si ya es un valor fijo), ajusta solo X y Y
        Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

        // Mover la cámara suavemente hacia la posición deseada
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
