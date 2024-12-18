using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class UIManagerMenu : MonoBehaviour
{
    public GameObject menuPanel;
    [SerializeField] private AudioSource ambientSound;
    private bool isSoundOn = true;
    public AudioSource clip;
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
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

    public void AnotherOptions()
    {

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
