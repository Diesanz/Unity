
using UnityEngine;

public class Victoria : MonoBehaviour
{
    /**
     * Referencia al componente Animator que controla las animaciones de la victoria.
     */
    private Animator animator;

    /**
     * La salud del enemigo, que se compara para determinar si ha sido derrotado.
     */
    public float health;

    /**
     * Referencia al script ObjectsManager que gestiona los objetos en el juego.
     */
    private ObjectsManager objectsManager;

    /**
     * Panel de victoria que se muestra cuando el jugador gana.
     */
    public GameObject victoryPanel;

    /**
     * Este método se llama al iniciar el juego.
     * Obtiene el componente Animator para controlar las animaciones del objeto.
     * También guarda una referencia al ObjectsManager y obtiene la salud del enemigo.
     */
    private void Start()
    {
        // Obtiene el componente Animator del objeto.
        animator = GetComponent<Animator>();

        // Guarda una referencia al ObjectsManager para evitar llamadas repetidas a FindObjectOfType.
        objectsManager = FindObjectOfType<ObjectsManager>();

        // Obtiene la salud del enemigo desde el componente Enemies.
        health = gameObject.GetComponent<Enemies>().vida;
    }

    /**
     * Este método se llama para verificar si el enemigo ha sido derrotado.
     * Si la salud del enemigo es menor o igual a 0, se muestra el panel de victoria y se detiene el tiempo del juego.
     */
    public void VictoriaEnemy()
    {
        // Actualiza la salud del enemigo para verificar si ha sido derrotado.
        health = gameObject.GetComponent<Enemies>().vida;

        // Verifica si la salud es menor o igual a 0, indicando que el enemigo ha sido derrotado.
        if (health <= 0)
        {
            // Detiene el tiempo del juego (el juego se pausa).
            Time.timeScale = 0;

            // Muestra el panel de victoria.
            victoryPanel.SetActive(true);
        }
    }
}
