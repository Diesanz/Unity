using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemies : MonoBehaviour
{
    public int rutina;
    public float cronometro;
    public Animator ani;
    public int direccion;
    public float speed_walk=2.0f;
    public GameObject target;
    public bool atacando;
    private Rigidbody2D rb;
    public float rango_vision;
    public float rango_ataque;
    public GameObject rango, hit;

    public float limiteIzquierdo;
    public float limiteDerecho;

    void Start()
    {
        ani = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Comportamiento();
    }

    public void Comportamiento()
    {
        if(Mathf.Abs(transform.position.x - target.transform.position.x) > rango_vision && !atacando)
        {
            cronometro += Time.deltaTime;
            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 3);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    ani.SetBool("walk", false);
                    break;
                case 1:
                    direccion = Random.Range(0, 2);
                    ani.SetBool("walk", true);
                    rutina = 2;
                    break;
                case 2:
                    Vector3 moveDir = Vector3.zero;
                    Vector3 nuevaPosicion = transform.position;

                    switch (direccion)
                    {
                        case 0: // Mover hacia la derecha
                            transform.rotation = Quaternion.Euler(0, 0, 0);
                            moveDir = transform.right * speed_walk * Time.deltaTime;
                            nuevaPosicion = transform.position + moveDir;
                            break;
                        case 1: // Mover hacia la izquierda
                            transform.rotation = Quaternion.Euler(0, 180, 0);
                            moveDir = transform.right * speed_walk * Time.deltaTime;
                            nuevaPosicion = transform.position + moveDir;
                            break;
                    }
                    // Verificar los límites antes de mover
                    if (nuevaPosicion.x >= limiteIzquierdo && nuevaPosicion.x <= limiteDerecho)
                    {
                        rb.MovePosition(nuevaPosicion);
                        ani.SetBool("walk", true);
                    }
                    else
                    {
                        // Si llega al límite, detener movimiento
                        ani.SetBool("walk", false);
                    }
                    break;
            }
        }
        else
        {
            if(Mathf.Abs(transform.position.x - target.transform.position.x) > rango_ataque && !atacando)
            {
                if(target.transform.position.x  >= limiteIzquierdo && target.transform.position.x <= limiteDerecho)
                {
                    if(transform.position.x < target.transform.position.x)
                    {
                        rb.MovePosition((transform.right * (speed_walk*2) * Time.deltaTime) + transform.position);
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        ani.SetBool("attack", false);
                    }
                    else{
                        rb.MovePosition((transform.right * (speed_walk*2) * Time.deltaTime) + transform.position);
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                        ani.SetBool("attack", false);
                    }
                    ani.SetBool("walk", true);
                }
                else{
                    ani.SetBool("walk", false);
                }

            }
            else
            {
                if(!atacando)
                {
                    if(transform.position.x < target.transform.position.x)
                    {
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                    ani.SetBool("walk", false);
                }
            }
        }
    }

    public void Final_ani()
    {
        ani.SetBool("attack", false);
        atacando = false;
        rango.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponTrue()
    {
        hit.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void ColliderWeaponFalse()
    {
        hit.GetComponent<BoxCollider2D>().enabled = false;
    }
}
