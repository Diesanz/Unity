using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Clase que controla la interacción con una puerta que permite al jugador 
 * abrir un nuevo nivel cuando está dentro del área y presiona una tecla específica.
 */
public class OpenLevel : MonoBehaviour
{
    /**
     * Referencia al objeto de texto que muestra un mensaje al jugador.
     */
    public Text text;

    /**
     * Nombre de la escena que se debe cargar al interactuar con la puerta.
     */
    public string levelName;

    /**
     * Indica si el jugador está dentro del área de interacción de la puerta.
     */
    private bool inDoor = false;

    /**
     * Evento que se activa cuando otro objeto entra en el área de colisión del trigger.
     * 
     * @param collision El collider del objeto que entra en el trigger.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprueba si el objeto es el jugador y si el texto no es nulo.
        if (collision.gameObject.CompareTag("Player") && text != null)
        {
            // Activa el mensaje si está desactivado.
            if (!text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(true);
            }
            // Marca que el jugador está dentro del área de interacción.
            inDoor = true;
        }
    }

    /**
     * Evento que se activa cuando otro objeto sale del área de colisión del trigger.
     * 
     * @param collision El collider del objeto que sale del trigger.
     */
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Comprueba si el objeto es el jugador y si el texto no es nulo.
        if (collision.gameObject.CompareTag("Player") && text != null)
        {
            // Desactiva el mensaje si está activo.
            if (text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(false);
            }
            // Marca que el jugador ha salido del área de interacción.
            inDoor = false;
        }
    }

    /**
     * Actualiza el estado en cada frame para verificar si el jugador 
     * presiona la tecla de interacción estando en el área.
     */
    private void Update()
    {
        // Carga la escena si el jugador está dentro y presiona la tecla "E".
        if (inDoor && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(levelName);

            // Limpia las preferencias guardadas para reiniciar configuraciones.
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }
    }
}
