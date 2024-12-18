using UnityEngine;
using UnityEngine.UI;

/**
 * Clase que gestiona un temporizador con límite de tiempo.
 * Muestra el tiempo restante en formato MM:SS y ejecuta acciones al finalizar el tiempo.
 */
public class Timer : MonoBehaviour
{
    /**
     * Límite de tiempo en segundos (por defecto 120 segundos, es decir, 2 minutos).
     */
    public float tiempoLimite = 120f;

    /**
     * Tiempo restante del temporizador.
     */
    private float tiempoRestante;

    /**
     * Referencia al objeto UI Text donde se muestra el tiempo restante.
     * Este debe asignarse en el Inspector.
     */
    public Text textoContador;

    /**
     * Indica si el tiempo ha llegado a su fin.
     */
    private bool tiempoTerminado = false;

    /**
     * Panel que se muestra cuando el tiempo termina. Debe asignarse en el Inspector.
     */
    public GameObject failPanel;

    /**
     * Inicializa el temporizador al inicio del juego.
     */
    void Start()
    {
        // Verificamos si la clave "TiempoRestante" existe en PlayerPrefs.
        if (PlayerPrefs.HasKey("TiempoRestante"))
        {
            // Si existe, cargamos el valor guardado.
            tiempoRestante = PlayerPrefs.GetFloat("TiempoRestante");
        }
        else
        {
            // Si no existe, asignamos el tiempo límite predeterminado.
            tiempoRestante = tiempoLimite;
        }

        // Mostrar el valor del tiempoRestante en el log para depuración.
        Debug.Log("Tiempo restante: " + tiempoRestante);
    }

    /**
     * Actualiza el estado del temporizador en cada frame.
     * Resta el tiempo transcurrido y maneja la lógica al terminar el tiempo.
     */
    void Update()
    {
        if (!tiempoTerminado)
        {
            // Solo actualiza el contador si el tiempo no ha terminado.
            if (tiempoRestante > 0)
            {
                // Resta el tiempo transcurrido al tiempo restante.
                tiempoRestante -= Time.deltaTime;

                // Actualiza el contador en la interfaz.
                ActualizarContador(tiempoRestante);
                PlayerPrefs.SetFloat("TiempoRestante", tiempoRestante);
            }
            else
            {
                // El temporizador llega a cero.
                tiempoRestante = 0;

                // Actualiza el contador para reflejar el tiempo final.
                ActualizarContador(tiempoRestante);

                // Maneja la lógica al terminar el tiempo.
                TiempoTerminado();
            }
        }
    }

    /**
     * Actualiza el texto del contador en la interfaz con el tiempo restante.
     *
     * @param tiempo Tiempo restante en segundos.
     */
    public void ActualizarContador(float tiempo)
    {
        // Calcula los minutos y segundos restantes.
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);

        // Actualiza el texto del contador en formato MM:SS.
        textoContador.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    /**
     * Lógica que se ejecuta cuando el tiempo se termina.
     * Detiene el juego y muestra el panel de fallo.
     */
    void TiempoTerminado()
    {
        // Cambia la bandera a true para evitar más actualizaciones.
        tiempoTerminado = true;

        // Pausa el tiempo en el juego.
        Time.timeScale = 0;

        // Activa el panel de fallo si está configurado.
        if (failPanel != null)
        {
            failPanel.SetActive(true);
        }
    }
}
