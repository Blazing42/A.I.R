using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlanNode
{
    public PlanNode parentNode;
    public float cost;
    public Dictionary<string, int> state;
    public GOAPAction action;

    public PlanNode(PlanNode parent, float cost, Dictionary<string,int> allStates, GOAPAction action)
    {
        this.parentNode = parent;
        this.cost = cost;
        //copy the dictionary into a new dictionary rather than just referencing the one that already exists
        this.state = new Dictionary<string, int>(allStates);
        this.action = action;
    }
}

public class GOAPPlanner 
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, Dictionary<string, int> goal, WorldStates states)
    {
        //make sure that all of the actions in the list are usable before putting them into the planner
        List<GOAPAction> usableActions = new List<GOAPAction>();
        foreach(GOAPAction a in actions)
        {
            if(a.IsAchievable())
            {
                usableActions.Add(a);
            }
        }

        //create a list of nodes to be the points on the graph
        List<PlanNode> graphNodes = new List<PlanNode>();
        //create the origin of the graph, an empty starting node that only contains the current world conditions/states
        PlanNode start = new PlanNode(null, 0, GOAPGameWorld.WorldInstance.GetWorld().GetWorldStates(), null);

        //build the graph of nodes joining all of the actions together to reach the goal,
        //creating various paths if possible from the current world state to the goal
        bool success = BuildGraph(start, graphNodes, usableActions, goal);
        //if the graph cant be built becuase there is no path from the current world conditions and the goal then lod an error and exit the method
        if(!success)
        {
            Debug.LogWarning("plan not found");
            return null;
        }
        else
        {
            Debug.LogWarning("plan found");
        }

        //in order to pick the cheapest path the cheapest node must be found and then worked back through the graph from
        //find the cheapest node
        PlanNode cheapest = null;
        foreach(PlanNode pn in graphNodes)
        {
            if(cheapest == null)
            {
                cheapest = pn;
            }
            else
            {
                if(pn.cost < cheapest.cost)
                {
                    cheapest = pn;
                }
            }
        }
        //create a list to hold the resulting actions
        List<GOAPAction> result = new List<GOAPAction>();
        PlanNode n = cheapest;
        {
            //if the cheapest node has been found
            while(n != null)
            {
                //and it is not the starting origin node
                if(n.action != null)
                {
                    //add the node to the front of the results list
                    result.Insert(0, n.action);
                }
                //get the parent node of the node that was added to the list and work through the wile loop again
                n = n.parentNode;
            }
        }

        //add the resulting list of actions to a queue that the agent can perform
        Queue<GOAPAction> queue = new Queue<GOAPAction>();
        foreach(GOAPAction a in result)
        {
            queue.Enqueue(a);
        }

        //debug the plan if one is found
        Debug.Log("The Plan is: ");
        foreach(GOAPAction q in queue)
        {
            Debug.Log("Q: " + q.actionName);
        }
        //finally return the queue
        return queue;

    }
    private bool BuildGraph(PlanNode parent, List<PlanNode> graphPoints, List<GOAPAction> actions, Dictionary<string,int> goalCond)
    {
        bool foundPath = false;
        foreach(GOAPAction a in actions)
        {
            if(a.IsAchievableGiven(parent.state))
            {
                //this copies the parent conditions keeping track of them as the branch is generated, 
                //doing a test run of the ai as it goes along to see if everything is possible
                Dictionary<string, int> currentstate = new Dictionary<string, int>(parent.state); 
                foreach(KeyValuePair<string,int> effects in a.aftereffects)
                {
                    //then if the effect of the current action is not already in the dictionary keeping track/simulating the world state at this point, add it
                    if(!currentstate.ContainsKey(effects.Key))
                    {
                        currentstate.Add(effects.Key, effects.Value);
                    }
                }
                //create a new node to represent the next action in the list of actions
                PlanNode node = new PlanNode(parent, parent.cost + a.cost, currentstate, a);
                //if this next action/node achieves the desired goal/world conditions
                if(GoalAchieved(goalCond, currentstate))
                {
                    //add the node to the graph
                    graphPoints.Add(node);
                    //and set found path to true
                    foundPath = true;
                }
                else
                {
                    //if this action node doesnt achieve the desired goal create a new list of actions without it
                    List<GOAPAction> subset = ActionSubset(actions, a);
                    //repeat the process with the new list of actions
                    bool found = BuildGraph(node, graphPoints, subset, goalCond);
                    if(found)
                    {
                        foundPath = true;
                    }
                }
            }
        }
        return foundPath;
    }
    //compare the goal world conditions to the current world conditions to see if all of the goal conditions have been met/achieved 
    private bool GoalAchieved(Dictionary<string,int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string,int> g in goal)
        {
            //if one of the goal conditions hasnt been met 
            if(!state.ContainsKey(g.Key))
            {
                //return false
                return false;
            }
        }
        //if all have been met return true
        return true;
    }

    //create a list of actions without the tested action to use in the next graph path
    private List<GOAPAction> ActionSubset(List<GOAPAction> availableActions, GOAPAction removeAction )
    {
        //create a new empty list
        List<GOAPAction> subset = new List<GOAPAction>();
        foreach(GOAPAction a in availableActions)
        {
            //fill it with all of the actions that arent the one that will be removed
            if(!a.Equals(removeAction))
            {
                subset.Add(a);
            }
        }
        //return the filled up list
        return subset;
    }
}
