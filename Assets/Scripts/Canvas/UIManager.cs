
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

/**
 * Clase que gestiona las funcionalidades de la interfaz de usuario (UI) del juego.
 * Incluye opciones para mostrar/ocultar menús, gestionar el sonido y cambiar de escenas.
 */
public class UIManager : MonoBehaviour
{
    /**
     * Panel de opciones que se muestra en el juego.
     * Debe ser asignado desde el Inspector.
     */
    public GameObject optionsPanel;

    /**
     * Fuente de audio para el sonido ambiental. Debe ser asignada en el Inspector.
     */
    [SerializeField] private AudioSource ambientSound;

    /**
     * Bandera para determinar si el sonido ambiental está activado o desactivado.
     */
    private bool isSoundOn = true;

    /**
     * Fuente de audio para reproducir clips de sonido específicos.
     */
    public AudioSource clip;

    /**
     * Muestra el panel de opciones y pausa el juego.
     */
    public void ShowOptions()
    {
        Time.timeScale = 0; // Pausar el tiempo del juego
        optionsPanel.SetActive(true); // Activar el panel de opciones
    }

    /**
     * Oculta el panel de opciones y reanuda el juego.
     */
    public void HideOptions()
    {
        Time.timeScale = 1; // Reanudar el tiempo del juego
        optionsPanel.SetActive(false); // Desactivar el panel de opciones
    }

    /**
     * Método placeholder para manejar otras opciones en el futuro.
     */
    public void AnotherOptions()
    {
        // Este método se puede implementar según necesidades futuras
    }

    /**
     * Cambia a la escena del menú principal y reanuda el tiempo del juego.
     */
    public void MainManu()
    {
        Time.timeScale = 1; // Reanudar el tiempo del juego
        SceneManager.LoadScene("MenuPrincipal"); // Cargar la escena del menú principal
    }

    /**
     * Cierra la aplicación.
     */
    public void QuitGame()
    {
        Application.Quit(); // Finaliza la ejecución del juego
    }

    /**
     * Cambia a la escena de selección de niveles y reanuda el tiempo del juego.
     * También borra las preferencias del jugador.
     */
    public void ShowLevels()
    {
        SceneManager.LoadScene("SelectLevel"); // Cargar la escena de selección de niveles
        PlayerPrefs.DeleteAll(); // Borrar las preferencias guardadas
        PlayerPrefs.Save(); // Guardar los cambios
        Time.timeScale = 1; // Reanudar el tiempo del juego
    }

    /**
     * Cambia a la escena principal del juego y reanuda el tiempo del juego.
     * También borra las preferencias del jugador.
     */
    public void PlayGame()
    {
        Time.timeScale = 1; // Reanudar el tiempo del juego
        SceneManager.LoadScene("SampleScene"); // Cargar la escena principal del juego
        PlayerPrefs.DeleteAll(); // Borrar las preferencias guardadas
        PlayerPrefs.Save(); // Guardar los cambios
    }

    /**
     * Reproduce un clip de audio específico.
     */
    public void PlaySound()
    {
        clip.Play(); // Reproducir el clip de audio
    }

    /**
     * Activa o desactiva el sonido ambiental.
     * Cambia el estado del sonido según la bandera `isSoundOn`.
     */
    public void ToggleSound()
    {
        // Cambiar el estado del sonido
        isSoundOn = !isSoundOn;

        // Activar o pausar el sonido ambiental según el estado actual
        if (isSoundOn)
        {
            ambientSound.Play(); // Reanudar el sonido
        }
        else
        {
            ambientSound.Pause(); // Pausar el sonido
        }
    }
}
