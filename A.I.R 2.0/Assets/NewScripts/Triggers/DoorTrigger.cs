using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A Class that uses Unity's trigger component system to ping the DoorEventSystem to trigger the door opening and closing events.
/// </summary>
public class DoorTrigger : MonoBehaviour
{
    public int triggerId;

    private void OnTriggerEnter(Collider other)
    {
        DoorEventSystem.current.DoorwayTriggerEnter(triggerId);
    }

    private void OnTriggerExit(Collider other)
    {
        DoorEventSystem.current.DoorwayTriggerExit(triggerId);
    }

}
