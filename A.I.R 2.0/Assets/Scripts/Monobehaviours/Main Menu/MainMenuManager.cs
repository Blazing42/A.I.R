using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
    GameManager gameManager;
    AudioSystem audioSystem;
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip clickSFX;
    [SerializeField] AudioClip hoverSFX;

    void Start()
    {
        gameManager = GameManager.Instance;
        audioSystem = AudioSystem.Instance;
        audioSystem.PlayBackgroundMusic(menuMusic);
    }
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
        StartCoroutine(PlaySoundAndLevelEditor());
    }

    public void OpenSettingsMenu()
    {
        //play a click sound effect using the sound system
        //load the settings menu
        StartCoroutine(PlaySoundAndSettings("Main Menu"));
    }

    public void OpenLevelOne()
    {
        //play a click sound effect using the sound system
        //load first level
        StartCoroutine(PlaySoundAndPlay());
    }

    public void QuitApplication()
    {
        //play a button click sound effect using the sound system
        //quit the application
        StartCoroutine(PlaySoundAndQuitApp());
    }

    public void PlayHoverSFX()
    {
        audioSystem.PlaySoundEffect(hoverSFX);
    }

    IEnumerator PlaySoundAndPlay()
    {
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        gameManager.LoadGame();
    }

    IEnumerator PlaySoundAndLevelEditor()
    {
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        gameManager.LoadLevelEditor();
    }

    IEnumerator PlaySoundAndSettings(string currentScene)
    {
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        gameManager.LoadSettings(currentScene);

    }

    IEnumerator PlaySoundAndQuitApp()
    {
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        Application.Quit();
    }
}
