using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    public string name = "defaultRoom";
    public Sprite roomImage;
    public bool gravityOn = true;
    public bool doorsLocked = false;
    public List<int> doorsToRoomIds;
    public float temperature = 20f;
    public float pressure = 1f;
    public float oxygenPercentageinAir = 25f;
    public float nitrogenParcentageinAir = 70f;
    public float carbonDioxidePercentageinAir = 3f;
    public float otherGassesPercentageinAir = 2f;
}
