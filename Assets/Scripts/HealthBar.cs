using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image bar;

    public void UpdateHealthBar(float vidaMAX, float vida)
    {
        bar.fillAmount = vida / vidaMAX;
    }
}
