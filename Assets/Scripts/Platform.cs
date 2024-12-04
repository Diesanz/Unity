using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;

    public float wait;

    private float waieted;
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp("s"))
        {
            waieted = wait;
        }

        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey("s"))
        {
            //esperar un tiempo para bajar de la plataforma
            if(waieted <= 0)
            {
                //cambiar efecto
                effector.rotationalOffset = 180f;
                waieted = wait; //resetear
            }
            else
            {
                //si no se pasa el tiempo lo disminuimos
                waieted -= Time.deltaTime;
            }
        }

        if(Input.GetKey(KeyCode.Space))
        {
            effector.rotationalOffset = 0;
        }
    }
}
