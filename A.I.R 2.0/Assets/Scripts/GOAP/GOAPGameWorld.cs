using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GOAPGameWorld
{
    private static readonly GOAPGameWorld worldInstance = new GOAPGameWorld();
    private static WorldStates world;
    //using queue for testing but a list for the final game
    private static Queue<GameObject> humansInRoom2;

    static GOAPGameWorld()
    {
        world = new WorldStates();
        humansInRoom2 = new Queue<GameObject>();
    }

    private GOAPGameWorld()
    {

    }

    public void AddHuman(GameObject crewMember)
    {
        humansInRoom2.Enqueue(crewMember);
    }

    public GameObject RemoveFirstHuman()
    {
        if(humansInRoom2.Count == 0)
        {
            return null;
        }
        else
        {
            return humansInRoom2.Dequeue();
        }
    }

    /*public void RemoveHuman(GameObject crewMember)
    {
        humansInRoom2.Dequeue(crewMember);
    }*/

    public static GOAPGameWorld WorldInstance
    {
        get { return worldInstance; }
    }
    public WorldStates GetWorld()
    {
        return world;
    }
}
