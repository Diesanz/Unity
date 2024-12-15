using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovimientoPlataformaenemigo : MonoBehaviour
{
    public float velocidad;
    public Transform controlador;
    [SerializeField] private float distancia;

    public bool movimientoDerecha;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D informacionSuelo = Physics2D.Raycast(controlador.position, Vector2.down, distancia);
        
        rb.velocity = new Vector2(velocidad, rb.velocity.y);

        if(informacionSuelo != false)
        {
            //Girar
            Girar();
        }
    }

    private void Girar()
    {
        movimientoDerecha = !movimientoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180);
        velocidad *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controlador.transform.position, controlador.transform.position + Vector3.down * distancia);
    }
}
