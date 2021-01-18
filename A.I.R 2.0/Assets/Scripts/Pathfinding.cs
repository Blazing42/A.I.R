using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding 
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    //grid that stores the nodes
    private Grid<PathfindingNode> pathfindingGrid;
    public Grid<PathfindingNode> PathfindingGrid { get { return pathfindingGrid; } }
    //nodes that havent been checked
    private List<PathfindingNode> openList;
    //nodes that have been checked
    private List<PathfindingNode> closedList;

    //constructor to create a new pathfinding grid
    public Pathfinding(int width, int height, float cellsize)
    {
        pathfindingGrid = new Grid<PathfindingNode>(width, height, cellsize, Vector3.zero, false,(Grid<PathfindingNode> pathfindingg, int x, int y) => new PathfindingNode(pathfindingg, x, y));
    }

    public List<PathfindingNode> FindPath(int startx, int starty, int endx, int endy)
    {
        //sets up the open and closed lists, as well as getting the start and end nodes
        PathfindingNode startNode = pathfindingGrid.GetGridObject(startx, starty);
        PathfindingNode endNode = pathfindingGrid.GetGridObject(endx, endy);
        openList = new List<PathfindingNode> { startNode };
        closedList = new List<PathfindingNode>();
        //resetting the grid after any previous times the pathfinding function has been called and the initial set up the first time
        //goes through each of the grid positions
        for (int x = 0; x < pathfindingGrid.width; x++)
        {
            for (int y = 0; y < pathfindingGrid.height; y++)
            {
                //gets the node in that position
                PathfindingNode node = pathfindingGrid.GetGridObject(x,y);
                //sets the gCost to infinite
                node.gCost = int.MaxValue;
                //caluculates a new fCost based on that
                node.CalculateFCost();
                //sets the previous node to null
                node.previousNode = null;
            }
        }
        //set the start node values to work from
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        //this is the checking loop that the pathfinding algorithm goes through
        //while there are still nodes that we havent checked
        while(openList.Count > 0)
        {
            //get the next lowest fcost node from the openlist to chack to see if it is the end node
            PathfindingNode currentNode = GetLowestFCostNode(openList);
            //Debug.Log(currentNode.X + " , " + currentNode.Y);
            if(currentNode == endNode)
            {
                //reached the final node;
                return CalculatePath(endNode);
            }
            //remove the checked node from the openlist and add it to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //goes through each of the nodes which surround the current node
            foreach(PathfindingNode neighbouringNode in GetNeighbouringPathfindingNodes(currentNode))
            {
                //if the node has already been checked move on to the next neighbour
                if(closedList.Contains(neighbouringNode))
                {
                    continue;
                }
                //if the node isnt walkable add it to the closed list and continue checking the other neighbours
                if(neighbouringNode.walkable == false)
                {
                    closedList.Add(neighbouringNode);
                    continue;
                }
                //update all of the values in the neighbouring node adding it to the open list if its gcost is less than its neighbours gcost
                int checkGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbouringNode);
                if(checkGCost < neighbouringNode.gCost)
                {
                    neighbouringNode.previousNode = currentNode;
                    neighbouringNode.gCost = checkGCost;
                    neighbouringNode.hCost = CalculateDistanceCost(neighbouringNode, endNode);
                    neighbouringNode.CalculateFCost();
                    //if this node isnt in the open list add it to the list
                    if(!openList.Contains(neighbouringNode))
                    {
                        //add it to the openList
                        openList.Add(neighbouringNode);
                    }
                }
            
            }
           
        }
        //out of nodes in the open list
        //no path was found
        Debug.LogWarning("Path not found");
        return null;
        
    }
    
    //method that takes the previous node values in each each node and returns it as a list of nodes
    List<PathfindingNode> CalculatePath(PathfindingNode endNode)
    {
            List<PathfindingNode> path = new List<PathfindingNode>();
            path.Add(endNode);
            PathfindingNode currentNode = endNode;
            while (currentNode.previousNode != null)
            {
                path.Add(currentNode.previousNode);
                currentNode = currentNode.previousNode;
            }
            path.Reverse();
            return path;
    }

    //method that returns a list of the nodes that surround the current node
    List<PathfindingNode> GetNeighbouringPathfindingNodes(PathfindingNode node)
    {
        List<PathfindingNode> neighbourList = new List<PathfindingNode>();
        //if the current node isnt on the leftmost edge of the grid
        if(node.X -1 >= 0)
        {
            //add the node to the left to the neighbour list
            neighbourList.Add(pathfindingGrid.GetGridObject(node.X - 1, node.Y));
            //add the node to the left and down to the neighbour list
            if (node.Y - 1 >= 0) 
                neighbourList.Add(pathfindingGrid.GetGridObject(node.X - 1, node.Y- 1));
            //add the node to the left and up to the neighbour list
            if(node.Y + 1 < pathfindingGrid.height) 
                neighbourList.Add(pathfindingGrid.GetGridObject(node.X - 1, node.Y + 1));
        }
        //if the current node isnt on the rightmost edge of the list
        if(node.X + 1 < pathfindingGrid.width)
        {
            // add the node to the right to the neighbour list
            neighbourList.Add(pathfindingGrid.GetGridObject(node.X + 1, node.Y));
            //add the node to the right and down to the neighbour list
            if(node.Y - 1 >= 0) 
                neighbourList.Add(pathfindingGrid.GetGridObject(node.X + 1, node.Y - 1));
            //add the node to the right and up to the neighbour list
            if (node.Y + 1 < pathfindingGrid.height) 
                neighbourList.Add(pathfindingGrid.GetGridObject(node.X + 1, node.Y + 1));
        }
        //if the current node isnt along the bottom edge of the grid
        if(node.Y - 1 >= 0)
        {
            //add the node directly below the current node on the grid to the neighbour list
            neighbourList.Add(pathfindingGrid.GetGridObject(node.X, node.Y - 1));
        }
        //if the current node isnt along the top edge of the grid
        if(node.Y + 1 < pathfindingGrid.height)
        {
            //add the node directly above the current node on the grid to the neighbour list
            neighbourList.Add(pathfindingGrid.GetGridObject(node.X, node.Y + 1));
        }
        //once all of the nodes have been checked and added return the list of neighbouring nodes
        return neighbourList;
    }

    //method to calculate the distance cost between the start and end value
    //used to calculate the hCost of nodes
    int CalculateDistanceCost(PathfindingNode a, PathfindingNode b)
    {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //takes a list of nodes and finds the one with the lowest fcost among them
    PathfindingNode GetLowestFCostNode(List<PathfindingNode> nodeList)
    {
        //initialy sets up the lowest fcost node to be the fist one in the list
        PathfindingNode lowestFCostNode = nodeList[0];
        //goes through each of the nodes in the list
        for (int i = 0; i < nodeList.Count; i++)
        {
            //checks if the current node has a lower fcost that the previous one that had the lowest fcost.
            if(nodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = nodeList[i];
            }
        }
        //once it has gone through all of the nodes in the list it returns the one with the lowest fcost
        return lowestFCostNode;
    }

    //method used to reset the walkability of the pathnodes
    //will be used to initially set up the pathfinding grid and to recalculate as the environment changes for different alien types
    //may need a pathfinding grid for each alien type or set up a dictionary mapping alien type/class to pathnode walkability
    public void SetWalkability(Grid<Tile> tileMapGrid)
    {
        //go through each of the pathfinding nodes
        for (int x = 0; x < pathfindingGrid.width; x++)
        {
            for (int y = 0; y < pathfindingGrid.height; y++)
            {
                //get the pathfindingnodes world position
               Vector3 gridPosition = pathfindingGrid.GetWorldPosition(x, y);
                //get the tile in that position
               Tile tile = tileMapGrid.GetGridObject(gridPosition);
                //check the type of tile it is
                if(tile.GetTileType() == Tile.TileType.Space)
                {
                    PathfindingNode node = pathfindingGrid.GetGridObject(x, y);
                    node.walkable = false;
                }
                else if(tile.GetTileType() == Tile.TileType.Floor)
                {
                    PathfindingNode node = pathfindingGrid.GetGridObject(x, y);
                    node.walkable = true;
                }
            }
        }
    }
}
