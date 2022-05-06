using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomValueSlider : MonoBehaviour, IPointerUpHandler
{
    public RoomControlPanelScript panel;

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        panel.ChangeTemperature();
    }
}
