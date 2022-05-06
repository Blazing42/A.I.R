using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that contains the methods that are triggered by the events in the tab button class, when the player mouses over tabs etc...
/// </summary>
public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Color idleColour;
    public Color hoverColour;
    public Color activeColour;
    public TabButton selectedTab;
    public List<GameObject> tabsToSwap;

    //adds the tab buttons to the 
    public void AddToButtonList(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }

    //method that triggers when the player mouses over an object
    public void OnTabEnter(TabButton button)
    {
        //sets the tabs back to original colour if its not the selected tab
        ResetTabButtons();

        //if tab is not selected it changes to the hover colour
        if(selectedTab == null || button != selectedTab)
        {
            button.background.color = hoverColour;
        }
        
    }

    //method that triggers when the player moves their mouse away from the tab
    public void OnTabExit(TabButton button)
    {
        ResetTabButtons();
    }

    //method that triggers when the player clicks on a tab
    public void OnTabSelected(TabButton button)
    {
        //sets this button as the selected tab
        selectedTab = button;
        //resets the other tab colours
        ResetTabButtons();
        //sets this button to the selected button colour
        button.background.color = activeColour;

        //sets the tab to the tab with the corresponding tab index while closing the other tabs
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < tabsToSwap.Count; i++)
        {
            if(i == index)
            {
                tabsToSwap[i].SetActive(true);
            }
            else
            {
                tabsToSwap[i].SetActive(false);
            }
        }
    }

    //method that sets all of the tabs back to their original idlecolour if they are not the selected tab
    public void ResetTabButtons()
    {
        foreach(TabButton tab in tabButtons)
        {
            if(selectedTab != null && tab == selectedTab)
            {
                continue;
            }
            tab.background.color = idleColour;
        }
                
    }

}

