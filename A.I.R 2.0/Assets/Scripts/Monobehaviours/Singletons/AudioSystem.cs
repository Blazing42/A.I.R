using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    //[SerializeField] AudioSource voiceSource;
    [SerializeField] AudioMixerGroup musicGroup1;
    //[SerializeField] AudioMixerGroup musicGroup2;
    [SerializeField] AudioMixerGroup soundEffect;
    [SerializeField] AudioMixerGroup voices;

    private static AudioSystem _instance;
    public static AudioSystem Instance { get { return _instance; } }


    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayBackgroundMusic(AudioClip backgroundMusic)
    {
        musicSource.clip = backgroundMusic;
        musicSource.loop = true;
        musicSource.Play();
        
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }

    public void PlaySoundEffect(AudioClip audioClip)
    {
        sfxSource.PlayOneShot(audioClip);
    }

    /*public void PlayVoiceLine(AudioClip voiceLine)
    {
        voiceSource.outputAudioMixerGroup = voices;
        voiceSource.PlayOneShot(voiceLine);
        voiceSource.outputAudioMixerGroup = musicGroup1;
    }*/

    

}
