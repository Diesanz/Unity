
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    /**
     * Imagen que representa la barra de salud en la interfaz de usuario.
     */
    [SerializeField] private Image bar;

    /**
     * Función que actualiza la barra de salud en la interfaz de usuario.
     * 
     * @param vidaMAX La cantidad máxima de salud del personaje.
     * @param vida La cantidad actual de salud del personaje.
     */
    public void UpdateHealthBar(float vidaMAX, float vida)
    {
        // Actualiza el valor de la barra de salud según el porcentaje de vida actual respecto a la vida máxima
        bar.fillAmount = vida / vidaMAX;
    }
}
