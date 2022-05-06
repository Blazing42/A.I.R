using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButtonUI : MonoBehaviour
{
    //variable used to control the UI audio
    AudioSystem audioSystem;
    GameManager gameManager;
    [SerializeField] AudioClip clickSFX;
    [SerializeField] AudioClip hoverSFX;
    [SerializeField] AudioClip congratulationsSFX;
    [SerializeField] AudioClip loseSFX;
    [SerializeField] AudioClip menuMusic;

    LevelSystem levelSystem;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    public GameObject menu;
    public GameObject UIPanel;

    // Start is called before the first frame update
    void Start()
    {
        //find the audiosystem and save a reference to it
        audioSystem = AudioSystem.Instance;
        //find the levelsystem and save a reference to it
        levelSystem = LevelSystem.Instance;
        //register the methods to the win and lose events in the level system
        gameManager = GameManager.Instance;
    }


    //methods to control the button behaviours of the win and lose screens
    //plays a sound when the buttons are hovered over
    public void PlayHoverSFX()
    {
        audioSystem.PlaySoundEffect(hoverSFX);
    }
    //takes the player to the main menu
    /*public void BackToMainMenu()
    {
        //play a click sound effect using the sound system
        //load main menu
        StartCoroutine(PlaySoundAndChangeScene("Main Menu"));
    }
    //reloads the level so that the player can have another go
    public void ReplayLevel()
    {
        //play a click sound effect using the sound system
        //load first level
        StartCoroutine(PlaySoundAndChangeScene("LevelOne"));
    }
    //quits the application
    public void QuitApplication()
    {
        //play a button click sound effect using the sound system
        //quit the application
        StartCoroutine(PlaySoundAndQuitApp());
    }


    //method to be triggered by an event sent by the level system, when the player wins the level
    public void OpenCloseWinPanel()
    {
        audioSystem.PlaySoundEffect(congratulationsSFX);
        winPanel.SetActive(!winPanel.activeSelf);
    }
    //method to be triggered by an event sent by the level system, when the player loses the level
    public void OpenCloseLosePanel()
    {
        audioSystem.PlaySoundEffect(loseSFX);
        losePanel.SetActive(!losePanel.activeSelf);
    }*/

    

    public void OpenMenu()
    {
        StartCoroutine(PlaySoundandOpenMenu());
    }

    public void OpenMainMenu()
    {
        StartCoroutine(PlaySoundandMainMenu());
    }

    public void ReplayLevel()
    {
        StartCoroutine(PlaySoundandReplay());
    }

    public void QuitGame()
    {
        StartCoroutine(PlaySoundAndQuitApp());
    }

    IEnumerator PlaySoundandOpenMenu()
    {
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        audioSystem.PlayBackgroundMusic(menuMusic);
        UIPanel.SetActive(!UIPanel.activeSelf);
        menu.SetActive(!menu.activeSelf);
        gameManager.PauseGame();
    }

    IEnumerator PlaySoundAndQuitApp()
    {
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        Application.Quit();
    }

    IEnumerator PlaySoundandReplay()
    {
        gameManager.UnPauseGame();
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        gameManager.ReplayLevel();
    }

    IEnumerator PlaySoundandMainMenu()
    {
        gameManager.UnPauseGame();
        audioSystem.PlaySoundEffect(clickSFX);
        yield return new WaitForSeconds(clickSFX.length);
        gameManager.LoadMainMenu("LevelOne");
    }
}
