using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    //Cuando el player pase se guarda toda la info como número de rayos, caramelos cogidos hasta ese momento
    private float checkpointPosX, checkpointPosY; //no se usa vector3 para guardar info en tiempo real, ya que palyer prefs no guarda vectores

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


}
