using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    public string actionName = "Action";
    //the cost of the action the higher it is the less the planner will want to do the action
    public float cost = 1.0f;
    //the position where the action will take place
    public GameObject target;
    public string targetTag;
    //how long the action will take to perform
    public float duration = 0;
    //the animation that will play while the action is taking place
    //public Animation animation;

    //the world conditions that need to be met for this action to be considered
    public WorldState[] preConditions;
    //the world conditions that will result from this action being performed
    public WorldState[] afterEffects;
    //once the conditions have been entered in the inspector convert them to dictionary so that they are easier to work with
    public Dictionary<string, int> preconditions;
    public Dictionary<string, int> aftereffects;

    public WorldStates agentStates;
    //is the action currently running
    public bool running = false;

    public GOAPAction()
    {
        preconditions = new Dictionary<string, int>();
        aftereffects = new Dictionary<string, int>();
    }

    public void Awake()
    {
        if(preConditions != null)
        {
            foreach(WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }
        if (afterEffects != null)
        {
            foreach(WorldState w in afterEffects)
            {
                aftereffects.Add(w.key, w.value);
            }
        }
    }

    //in the future add some functionality to these two methods that means that some actions will only be considered after particular 
    //effects and things have been triggered e.g. a certain number of the aliens have died due to cold etc
    public bool IsAchievable()
    {
        return true;
    }

    //is the action achievable given the conditions passed into the method in the form of a dictionary
    public bool IsAchievableGiven(Dictionary<string,int> conditions)
    {
        foreach(KeyValuePair<string,int>p in preconditions)
        {
            if(!conditions.ContainsKey(p.Key))
            {
                return false;
            }
        }
        return true;
    }

    //methods to allow for customised code for each action that might happen before or after the "action" takes place
    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
