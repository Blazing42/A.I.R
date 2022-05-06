using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Script that uses the unity event system to trigger events when the player mouses over and clicks on tabs
/// </summary>
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    public Image background;

    // Start is called before the first frame update
    void Start()
    {
        //asigns the image on this objects to the background variable
        background = GetComponent<Image>();
        //adds this button to the tabgroup observer list
        tabGroup.AddToButtonList(this);
    }

    //triggers an event when the player moves their mouse over this object
    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    //triggers an event when the player clicks on the object
    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    //triggers an event when the player moves their mouse away from the object
    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
}
