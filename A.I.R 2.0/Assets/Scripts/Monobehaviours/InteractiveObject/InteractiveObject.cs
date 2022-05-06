using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class InteractiveObject : MonoBehaviour
{
    List<PathfindingNode> nodeBase;
    public List<PathfindingNode> surroundingNodes;
    public Grid<PathfindingNode> pathfindingGrid;
    public int maxHp = 100;
    public int currentHp;

    public LevelSystem levelSystem;

    // Start is called before the first frame update
    void Awake()
    {
        SetupInteractiveObject();
    }

    public void SetupInteractiveObject()
    {
        levelSystem = LevelSystem.Instance;
        //Debug.Log(levelSystem);
        pathfindingGrid = levelSystem.pathfindingGrid.PathfindingGrid;
        //Debug.Log(pathfindingGrid);
        currentHp = maxHp;
        nodeBase = SetBase();
        surroundingNodes = SetSurroundingNodes();
    }

    //sets up a basic base for the object so that creatures wouln't walk through the, base Set base sets a single floortiles worth of pathfinding nodes (4) unwalkable
    public List<PathfindingNode> SetBase()
    {
        //create a new empty list to contain all of the base nodes
        List<PathfindingNode> baseNodes = new List<PathfindingNode>();
        //get the node at the base of the sprite to be the base node
        PathfindingNode baseNode = pathfindingGrid.GetGridObject(transform.position);
        //add the base node to the list
        baseNodes.Add(baseNode);
        //add the node to the upper right to the list
        baseNodes.Add(pathfindingGrid.GetGridObject(baseNode.X + 1, baseNode.Y));
        //add the node to the upper left to the list
        baseNodes.Add(pathfindingGrid.GetGridObject(baseNode.X, baseNode.Y + 1));
        //add the node abovet the base node to the list
        baseNodes.Add(pathfindingGrid.GetGridObject(baseNode.X + 1, baseNode.Y + 1));

        //make sure to set all of the base nodes to be unwalkable to that creatures dont walk through them
        /*foreach(PathfindingNode node in baseNodes)
        {
            node.walkable = false;
        }*/
        return baseNodes;
    }

    //method used to get the surrounding nodes, needs to be altered to allow for objects larger that a tile in size!!!
    public List<PathfindingNode> SetSurroundingNodes()
    {
        //create an empty list of nodes to contane references to all of the nodes added during the GetNeighbouting nodes method
        List<PathfindingNode> nonDistinctListNodes = new List<PathfindingNode>();
        //fill in the empty list
        foreach(PathfindingNode node in nodeBase)
        {
            nonDistinctListNodes.AddRange(GetNeighbouringNodes(node));
        }
        //create a new list without any repeates so there is only one reference to each node in the list
        List<PathfindingNode> distinctNodeList = nonDistinctListNodes.Distinct().ToList();
        //remove the baseNodes from the new list to get the surrounding nodes
        distinctNodeList.Remove(nodeBase[0]);
        distinctNodeList.Remove(nodeBase[1]);
        distinctNodeList.Remove(nodeBase[2]);
        distinctNodeList.Remove(nodeBase[3]);
        //return the list of surrounding nodes
        return distinctNodeList;
    }

    PathfindingNode[] GetNeighbouringNodes(PathfindingNode node)
    {
        PathfindingNode[] neighbouringNodes = new PathfindingNode[8];
        neighbouringNodes[0] = pathfindingGrid.GetGridObject(node.X - 1, node.Y - 1);
        neighbouringNodes[1] = pathfindingGrid.GetGridObject(node.X - 1, node.Y);
        neighbouringNodes[2] = pathfindingGrid.GetGridObject(node.X, node.Y - 1);
        neighbouringNodes[3] = pathfindingGrid.GetGridObject(node.X + 1, node.Y - 1);
        neighbouringNodes[4] = pathfindingGrid.GetGridObject(node.X - 1, node.Y + 1);
        neighbouringNodes[5] = pathfindingGrid.GetGridObject(node.X + 1, node.Y + 1);
        neighbouringNodes[6] = pathfindingGrid.GetGridObject(node.X + 1, node.Y);
        neighbouringNodes[7] = pathfindingGrid.GetGridObject(node.X, node.Y + 1);
        return neighbouringNodes;
    }

    public virtual void TakeDamage(int damageAmount)
    {
        currentHp -= damageAmount;
    }

}
