using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtilities 
{
    //converts the mouse input position on the screen to the unity world position
    public static Vector3 ScreenToWorldPoint(Vector3 screenPos, Camera camera)
    {
        Vector3 worldPosition = camera.ScreenToWorldPoint(screenPos);
        return worldPosition;
    }
}
