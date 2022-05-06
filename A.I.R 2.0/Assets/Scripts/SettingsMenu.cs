using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    //start pauses the game and sets up resolutions
    void Start()
    {
        //set the timescale to 0 so that everything else in the game is paused while canging the settings
        Time.timeScale = 0;
        //gets an array of resolutions that the current computer can support
        resolutions = Screen.resolutions;
        //clear the dropdown list
        resolutionDropdown.ClearOptions();
        //set up the current resolution index
        int currentResolutionIndex = 0;
        //creat an empty list of strings
        List<string> resolutionOptions = new List<string>();
        //fill the list of strings with the resolutions
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resolutionOptions.Add(option);
            //set the current resolution index to be resolution of the screen
            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        //add all of the strings to dropdown options
        resolutionDropdown.AddOptions(resolutionOptions);
        //set the current options to the screen resolution
        resolutionDropdown.value = currentResolutionIndex;
        //make sure that the current value is show on the dropdown list
        resolutionDropdown.RefreshShownValue();
    }


    //graphics tab methods
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    //set the screen resolution
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //sets the game to fullscreen mode
    public void SetFullscreen(bool isfullscreen)
    {
        Screen.fullScreen = isfullscreen;
    }

    //sets the antialiasing 
    public void SetAntialiasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex * aaIndex;
    }

    //sets how often the game v syncs
    public void SetVsync(int vsyncIndex)
    {
        QualitySettings.vSyncCount = vsyncIndex;
    }

    //sets the quality of the textures in game
    public void SetTextureQuality(int textureQualityIndex)
    {
        QualitySettings.masterTextureLimit = textureQualityIndex;
    }

    //sets the game brightness
    public void SetBrighness(float brightnesslv)
    {
        //set the game brightness
    }


    //audio tab methods
    public AudioMixer audioMixer;

    //methods that control the volume sliders in the audio tab of the settings menu
    //master volume
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    //music volume
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }
    //sound effects volume
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }
    //voice volume if and when we add them
    public void SetVoiceVolume(float volume)
    {
        audioMixer.SetFloat("VoiceVolume", volume);
    }

    //method that controls whether the subtitles are on or off also part of the audio tab
    public void ToggleSubtitles(bool toggle)
    {
        //turn subtitles on or off
    }

    //methods that are accessable from all of the tabs
    //method that saves all of these values into the player prefs or other save system
    public void SaveSettingButtonPressed()
    {
        //save the current settings so they are used next time the same player logs in to the game
    }

    //method that exits the menu
    public void ExitButtonPressed()
    {
        //closes the current settings menu scene that is additiv on top of play
        //sets the timescale so that it is back to normal
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
