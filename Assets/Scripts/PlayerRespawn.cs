using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    public GameObject[] corazones;
    private int life;
    public float invulnerabilityDuration = 0.75f; // Tiempo de invulnerabilidad tras recibir daño
    private bool isInvulnerable = false; // Controla si el jugador puede recibir daño
    private float checkpointPosX, checkpointPosY;
    public Animator animator;
    public float respawnDelay = 0.5f; // Tiempo de espera antes de reiniciar la escena

    void Start()
    {
        life = corazones.Length;
        if (PlayerPrefs.GetFloat("checkpointPosX") != 0)
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat("checkpointPosX"), PlayerPrefs.GetFloat("checkpointPosY"));
        }
    }

    public void CheckLive()
    {
        if (life < 1)
        {
            Destroy(corazones[0].gameObject);
            StartCoroutine(HandlePlayerDeath());
        }
        else if (life < 2)
        {
            Destroy(corazones[1].gameObject);
            animator.Play("Hit");
        }
        else if (life < 3)
        {
            Destroy(corazones[2].gameObject);
            animator.Play("Hit");
        }
    }

    public void ReachedCheckpoint(float x, float y)
    {
        PlayerPrefs.SetFloat("checkpointPosX", x);
        PlayerPrefs.SetFloat("checkpointPosY", y);
    }

    public void PlayerDamaged()
    {
        if (!isInvulnerable) // Solo recibe daño si no es invulnerable
        {
            life--;
            CheckLive();
            StartCoroutine(InvulnerabilityTimer());
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true; // Activa la invulnerabilidad


        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false; // Desactiva la invulnerabilidad

    }

    private IEnumerator HandlePlayerDeath()
    {
        animator.Play("Dead");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;
        yield return new WaitForSeconds(animationDuration);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
