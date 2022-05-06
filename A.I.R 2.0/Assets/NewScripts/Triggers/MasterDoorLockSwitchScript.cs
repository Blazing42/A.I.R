using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterDoorLockSwitchScript : MonoBehaviour
{
    Toggle doorLockToggle;
    bool doorLockOn;

    void Start()
    {
        doorLockToggle = this.GetComponent<Toggle>();
        doorLockOn = doorLockToggle.isOn;
    }
   public void OnMasterLockDoorToggleChanged()
    {
        if(!doorLockOn)
        {
            if(EnergySystem.current.EnoughEnergyToLockAllDoors() == true)
            {
                DoorEventSystem.current.LockAllDoors();
                EnergySystem.current.SpendEnergy();
                doorLockOn = doorLockToggle.isOn;
            }
            else
            {
                doorLockToggle.isOn = false;
            }
            
        }
        else
        {
            if (EnergySystem.current.EnoughEnergyToLockAllDoors() == true)
            {
                DoorEventSystem.current.UnlockAllDoors();
                EnergySystem.current.SpendEnergy();
                doorLockOn = doorLockToggle.isOn;
            }
            else
            {
                doorLockToggle.isOn = true;
            }
        }
    }
}
