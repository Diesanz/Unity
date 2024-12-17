using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject optionsPanel;

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

}
