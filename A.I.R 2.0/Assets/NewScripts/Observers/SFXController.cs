using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip doorSFX;

    // Start is called before the first frame update
    void Start()
    {
        DoorEventSystem.current.onDoorwayTriggerEnter += PlayDoorSFX;
        DoorEventSystem.current.onDoorwayTriggerExit += PlayDoorSFX;
        if(audioSource == null)
        {
            audioSource = Camera.main.GetComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void PlayDoorSFX(int id)
    {
        audioSource.PlayOneShot(doorSFX);
    }

   
    void onDestroy()
    {
        DoorEventSystem.current.onDoorwayTriggerEnter -= PlayDoorSFX;
        DoorEventSystem.current.onDoorwayTriggerExit -= PlayDoorSFX;
    }
}
