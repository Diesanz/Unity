using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victoria : MonoBehaviour
{
    private Animator animator;
    public float health;
    private ObjectsManager objectsManager;
    public GameObject victoryPanel;

    private void Start()
    {
        // Obtiene el componente Animator, gameObject representa el objeto obtenido 
        animator = GetComponent<Animator>();
        // Guardar una referencia al ObjectsManager para evitar llamar a FindObjectOfType repetidamente
        objectsManager = FindObjectOfType<ObjectsManager>();
        health = gameObject.GetComponent<Enemies>().vida;
    }

    void Update(){
        health = gameObject.GetComponent<Enemies>().vida;
        if(health <= 0){
            Debug.LogWarning("En");
            victoryPanel.SetActive(true);
        }
    }

}
