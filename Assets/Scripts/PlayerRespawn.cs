using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    //Cuando el player pase se guarda toda la info como número de rayos, caramelos cogidos hasta ese momento
    private float checkpointPosX, checkpointPosY; //no se usa vector3 para guardar info en tiempo real, ya que palyer prefs no guarda vectores
    public Animator animator;
    public float respawnDelay = 0.5f; // Tiempo de espera antes de reiniciar la escena
    void Start()
    {
        //Posicion x e y para respawn
        if(PlayerPrefs.GetFloat("checkpointPosX") != 0)
        {
            transform.position=new Vector2(PlayerPrefs.GetFloat("checkpointPosX"), PlayerPrefs.GetFloat("checkpointPosY"));
        }
    }

    /*se llama cuando se pase por el checkpoint
    *@param x valor de coordenada x de donde esta el checkpoint por donde se ha pasado
    *@param y valor de coordenada y de donde esta el checkpoint por donde se ha pasado
    */
    public void ReachedCheckpoint(float x, float y)
    {
        PlayerPrefs.SetFloat("checkpointPosX",x); //Guarda coordenada x de donde esta el checkpoint
        PlayerPrefs.SetFloat("checkpointPosY",y);
    }

    public void PlayerDamaged()
    {
        StartCoroutine(HandlePlayerDeath());
    }

    private IEnumerator HandlePlayerDeath()
    {
        // Activar la animación de muerte
        animator.Play("Dead");

        // Obtener la duración de la animación "Dead"
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        // Esperar la duración de la animación
        yield return new WaitForSeconds(animationDuration);

        // Reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
