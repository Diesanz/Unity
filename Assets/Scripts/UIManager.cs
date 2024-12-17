using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public GameObject optionsPanel;
    [SerializeField] private AudioSource ambientSound;
    private bool isSoundOn = true;
    public AudioSource clip;

    public void ShowOptions()
    {
        Time.timeScale = 0;
        optionsPanel.SetActive(true);
    }

    public void HideOptions()
    {
        Time.timeScale = 1;
        optionsPanel.SetActive(false);
    }

    public void AnotherOptions()
    {

    }

    public void MainManu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowLevels()
    {
        
        SceneManager.LoadScene("SelectLevel");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void PlaySound()
    {
        clip.Play();
    }

    public void ToggleSound()
    {
        // Cambia el estado del sonido
        isSoundOn = !isSoundOn;

        // Activa o desactiva el sonido
        if (isSoundOn)
        {
            ambientSound.Play();
        }
        else
        {
            ambientSound.Pause();
        }
    }

}
