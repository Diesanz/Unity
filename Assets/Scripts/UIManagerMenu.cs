using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManagerMenu : MonoBehaviour
{
    public GameObject menuPanel;

    public void PlayGame()
    {
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
        SceneManager.LoadScene("Levels");
    }

    public void AnotherOptions()
    {

    }


}
