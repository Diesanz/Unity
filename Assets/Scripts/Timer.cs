using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float tiempoLimite = 120f; // 2 minutos en segundos
    private float tiempoRestante;
    public Text textoContador; // Asignar el UI Text en el Inspector para mostrar el tiempo
    private bool tiempoTerminado = false; // Bandera para saber si el tiempo ha terminado

    public GameObject failPanel; // Panel de fallo para mostrar al terminar el tiempo

    void Start()
    {
        // Inicializamos el tiempo restante con el valor límite
        tiempoRestante = tiempoLimite;
    }

    void Update()
    {
        if (!tiempoTerminado)
        {
            // Solo actualizar el contador si el tiempo no ha terminado
            if (tiempoRestante > 0)
            {
                // Restamos el tiempo transcurrido
                tiempoRestante -= Time.deltaTime;
                
                // Actualizamos el contador
                ActualizarContador(tiempoRestante);
            }
            else
            {
                // El contador ha llegado a cero
                tiempoRestante = 0;
                ActualizarContador(tiempoRestante);
                TiempoTerminado(); // Llamamos a la función que maneja lo que pasa cuando el tiempo se acaba
            }
        }
    }

    void ActualizarContador(float tiempo)
    {
        // Calcula los minutos y segundos restantes
        int minutos = Mathf.FloorToInt(tiempo / 60);
        int segundos = Mathf.FloorToInt(tiempo % 60);

        // Muestra el tiempo en formato MM:SS en el UI
        textoContador.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void TiempoTerminado()
    {
        // Cambia la bandera a true para que no se vuelva a actualizar
        tiempoTerminado = true;

        // Pausar el tiempo en el juego
        Time.timeScale = 0;

        // Mostrar el panel de fallo
        if (failPanel != null)
        {
            failPanel.SetActive(true);
        }
    }
}
