using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitApplication();
        }
    }

    //methods used to control the buttons in the main menu
    public void OpenLevelEditor()
    {
        //play a click sound effect using the sound system
        //load in the new scene
        OpenScene("Level Editor");
    }

    //method used to load in new scenes using their names
    void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApplication()
    {
        //play a button click sound effect using the sound system
        //quit the application
        Application.Quit();
    }
}
