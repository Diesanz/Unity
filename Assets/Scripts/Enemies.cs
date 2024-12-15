using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Video;

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
    public int vida = 1;
     private bool estaMuerto = false; // Bandera para evitar que el enemigo muera varias veces.
    private float tiempoEntreGolpes = 0.5f; // Tiempo de cooldown entre golpes
    private float ultimoGolpe = 0f; // Registro del último golpe

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
        if (ani.GetBool("death"))
        {
            return; // Salir del método si está muerto
        }

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bola") && !estaMuerto) // Verifica si ya está muerto
        {
            // Control de cooldown para que no reciba daño repetidamente en un corto tiempo
            if (Time.time - ultimoGolpe < tiempoEntreGolpes) return;
            ultimoGolpe = Time.time;

            if (!ani.GetCurrentAnimatorStateInfo(0).IsName("hurt"))
            {
                vida--; // Resta 1 a la vida
                Debug.LogWarning(vida);

                if (vida > 0)
                {
                    ani.SetTrigger("hurt"); // Ejecuta la animación de daño
                }
                else
                {
                    // Si la vida es 0 o menos, activa la animación de muerte
                    ani.SetBool("death", true);
                    estaMuerto = true; // Marca al enemigo como muerto
                    StartCoroutine(DestruirEnemigo()); // Espera a que termine la animación y destruye el enemigo
                }

                // Dependiendo del estado de la animación actual, ajustamos las animaciones de movimiento o ataque
                if (ani.GetBool("walk"))
                {
                    ani.SetBool("walk", true); // Vuelve a Walk
                }
                else if (ani.GetBool("attack"))
                {
                    ani.SetBool("attack", true);
                }
                else
                {
                    ani.SetBool("walk", false); // Vuelve a Idle
                    ani.SetBool("attack", false);
                }
            }
        }
    }

    // Coroutine para destruir al enemigo después de la animación de muerte
    IEnumerator DestruirEnemigo()
    {
        // Espera la duración de la animación actual (Muerte)
        yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // Destruye el enemigo
    }
}
