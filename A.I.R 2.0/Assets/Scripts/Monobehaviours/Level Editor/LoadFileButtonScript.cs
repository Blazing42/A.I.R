﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadFileButtonScript : MonoBehaviour
{
    public void LoadFile()
    {
        string loadfilename = GetComponentInChildren<Text>().text;
        LevelEditorSystem test = GameObject.FindObjectOfType<LevelEditorSystem>();
        LevelEditorButtonBehaviours buttonBehaviours = GameObject.FindObjectOfType<LevelEditorButtonBehaviours>();
        //Debug.Log(test.ToString());
        test.floorTileMap.LoadTileMap(loadfilename);
        test.roomGrid.LoadRooms(loadfilename);
        buttonBehaviours.LoadButtonPanel.SetActive(false);
        test.pathfindingGrid.SetWalkability(test.floorTileMap.tileGrid);
    }
}