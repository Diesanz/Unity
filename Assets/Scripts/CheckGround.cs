using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public static bool isGround;

    private void OnTriggerEnter2D(Collider2D collisiom)
    {
        isGround = true;
    }

    private void OnTriggerExit2D(Collider2D collisiom)
    {
        isGround = false;
    }
}
