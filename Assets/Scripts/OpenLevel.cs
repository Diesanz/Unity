using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpenLevel : MonoBehaviour
{
    public Text text;
    public string levelName;
    private bool inDoor = false;

    // Configurar la tecla de interacción desde el Inspector
    public KeyCode interactKey = KeyCode.E;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica si el objeto Text no es nulo antes de intentar acceder a él
        if (collision.gameObject.CompareTag("Player") && text != null)
        {
            if (!text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(true);
            }
            inDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Verifica si el objeto Text no es nulo antes de intentar acceder a él
        if (collision.gameObject.CompareTag("Player") && text != null)
        {
            if (text.gameObject.activeSelf)
            {
                text.gameObject.SetActive(false);
            }
            inDoor = false;
        }
    }

    private void Update()
    {
        // Si el jugador está dentro y presiona la tecla, cargar la escena
        if (inDoor && Input.GetKeyDown(interactKey)) // Usar GetKeyDown para que la acción sea por una sola vez
        {
            SceneManager.LoadScene(levelName);
        }
    }
}
