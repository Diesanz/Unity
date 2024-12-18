using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Clase que gestiona los objetos en la escena, verificando si quedan objetos como hijos de un GameObject.
 * Cuando no hay más objetos, puede usarse para indicar que se ha alcanzado la condición de victoria.
 */
public class ObjectsManager : MonoBehaviour
{
    /**
     * Método llamado en cada frame para verificar el número de objetos hijos del GameObject asociado.
     */
    private void Update()
    {
        // Llama a la función para verificar el número de objetos hijos.
        AllObjectsNumber();
    }

    /**
     * Verifica si el GameObject actual no tiene hijos.
     * Si no hay hijos, imprime "Victoria" en la consola.
     */
    public void AllObjectsNumber()
    {
        if (transform.childCount == 0)
        {
            // Indica en la consola que se ha cumplido la condición de victoria.
            Debug.Log("Victoria");
        }
    }
}
