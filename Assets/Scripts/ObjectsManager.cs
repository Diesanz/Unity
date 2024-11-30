using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsManager : MonoBehaviour
{
    //Mirar si dentro de la carpeta hay hijos

    private void Update()
    {
        AllObjectsNumber();
    }
    public void AllObjectsNumber()
    {
        if(transform.childCount==0)
        {
            Debug.Log("Victoria");
        }
    }
    //Se puede meter en un update pero no es obtimo
}
