using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this struct shows how the current world condition is saved as a key value pair, 
//the string e.g. food is available and an int value usually used for resources e.g. 5 food available
[System.Serializable]
public struct WorldState
{
    public string key;
    public int value;
}

//the world states class is used to store the world state key value pairs in the form of a dictionary, 
//and also to contain the methods used to organise and update this dictionary as the game progresses
public class WorldStates
{
    //the dictionary that stores all of the world states
    public Dictionary<string, int> worldStates;

    //constructor to instantiate this class
    public WorldStates()
    {
        worldStates = new Dictionary<string, int>();
    }

    //method used to return the dictionary, used by the planner to get all of the world states to formulate a plan
    public Dictionary<string,int> GetWorldStates()
    {
        return worldStates;
    }

    ///management methods use to change the contents of the dictionary
    //method used to determine if the world states dictionary already contains a key value pair with the supplied key
    public bool HasWorldState(string key)
    {
        return worldStates.ContainsKey(key);
    }

    //method used to add a new world state to the dictionary
    void AddState(string key, int value)
    {
        worldStates.Add(key, value);
    }

    //method used to remove a world state from the dictionary entirely
    public void RemoveState(string key)
    {
        if(worldStates.ContainsKey(key))
        {
            worldStates.Remove(key);
        }
    }

    //method used to change the value of the int assosiated with a world state key in the dictionary by the modvalue, for example when a resource is used up
    public void ModifyStateValue(string key, int modvalue)
    {
        //if the world states dict contains the key
        if(worldStates.ContainsKey(key))
        {
            //change the value by the int modvalue/ modification value 
            worldStates[key] += modvalue;
            if(worldStates[key] <= 0)
            {
                RemoveState(key);
            }
        }
        //if the world state doesnt already exist add it to the dictionary
        else
        {
            worldStates.Add(key, modvalue);
        }
    }

    //method used to set the value of the int assosiated with a world state key in the dictionary to the newvalue
    public void SetStateValue(string key, int newvalue)
    {
        if(worldStates.ContainsKey(key))
        {
            worldStates[key] = newvalue;
        }
        else
        {
            worldStates.Add(key, newvalue);
        }
    }
}
