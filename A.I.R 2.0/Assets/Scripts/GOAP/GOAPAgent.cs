using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//small struct to hold the agents goal
public struct SubGoal
{
    //dictionary that contains all the conditions that need to be met for the agent to have completed this subgoal
    public Dictionary<string, int> sGoals;
    //bool to determine if it is a repeatable goal or not
    public bool remove;

    //constructor
    public SubGoal(string s, int i, bool r)
    {
        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r;
    }
}

public class GOAPAgent : MonoBehaviour
{
    //list of actions that the agent can perform 
    public List<GOAPAction> actions = new List<GOAPAction>();
    //list of goals that the agent wants to complete
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();

    //reference to the brain/planner
    GOAPPlanner planner;
    //the action queue that is recieved from the planner to get to the current goal
    Queue<GOAPAction> actionQueue;
    //reference to the action that the agent is currently in the middle of performing
    public GOAPAction currentAction;
    SubGoal currentGoal;
    //Reference to the movement handler
    GOAPAgentMovementHandler agentMovementHandler;

    // Start is called before the first frame update
    public void Start()
    {
        //get all of the actions that this creature can complete from the components and add to this list
        GOAPAction[] acts = this.GetComponents<GOAPAction>();
        //get the agent movement handler if not assigned in the inspector
        if(agentMovementHandler == null)
        {
            agentMovementHandler = this.GetComponent<GOAPAgentMovementHandler>();
        }
        foreach(GOAPAction a in acts)
        {
            actions.Add(a);
        }
    }

    bool invoked = false;

    void CompleteAction()
    {
        currentAction.running = false;
        currentAction.PostPerform();
        invoked = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (currentAction != null && currentAction.running)
        {
            if (agentMovementHandler.pathVectorList != null && agentMovementHandler.RemainingDistance(currentAction.target.transform.position) < 1.0f) 
            { 
                //check to see if the agent has reached the place where it needs to perform the action
                if (!invoked)
                {
                    Invoke("CompleteAction", currentAction.duration);
                    invoked = true;
                }
                
            }
            return;
        }

        //if the planner has been reset of hasnt already been set up
        if(planner == null || actionQueue == null)
        {
            //create a new planner reference
            planner = new GOAPPlanner();
            //sort the subgoals into order of importance
            var sortedGoals = from entry in goals orderby entry.Value descending select entry;
            //go through each of the goals and create plans to see if they are possible
            foreach(KeyValuePair<SubGoal,int> sg in sortedGoals)
            {
                //if there is a possible plan
                actionQueue = planner.Plan(actions, sg.Key.sGoals, null);
                //pass the queue of actions to complete the plan into the agent
                if(actionQueue != null)
                {
                    currentGoal = sg.Key;
                    break;
                }
            }
        }

        //if the action queue is not equal to null and the agent has reached the end of the action queue the goal has been reached
        if(actionQueue != null && actionQueue.Count == 0)
        {
            //if the current subgoal is unrepeatable
            if(currentGoal.remove)
            {
                //remove it from the list of goals
                goals.Remove(currentGoal);
            }
            //reset the planner to start again with the next goal
            planner = null;
        }

        //if the action queue has actions in it
        if(actionQueue != null && actionQueue.Count > 0)
        {
            //set the current action to the next one in the queue
            currentAction = actionQueue.Dequeue();
            //complete the preperformance checks to see if the action is doable based on resources
            if(currentAction.PrePerform())
            {
                //if the action is doable make sure the agent has a target to move towards to complete the action
                if(currentAction.target == null && currentAction.targetTag != "")
                {
                    currentAction.target = GameObject.FindGameObjectWithTag(currentAction.targetTag);
                }
                //if the target is found
                if(currentAction.target != null)
                {
                    //set the action to running
                    currentAction.running = true;
                    //set the creature to move to the destination set in the action
                    agentMovementHandler.SetTargetPosition(currentAction.target.transform.position);
                }
            }
            else
            {
                //if this action isnt doable because of the preperformance actions create a new plan
                //makes sure the agent doesnt get stuck
                actionQueue = null;
            }
        }
    }
}
