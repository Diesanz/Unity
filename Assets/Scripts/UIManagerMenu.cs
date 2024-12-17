using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class UIManagerMenu : MonoBehaviour
{
    public GameObject menuPanel;
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



}
