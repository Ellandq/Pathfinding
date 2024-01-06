using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace Pathfinding;

public class Pathfinding <T> where T : PathNode
{
    // Movement Grid
    private T[,] nodeGrid;
    
    // Movement Values
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    public int CheckedNodeCounter { get; private set; }
    public static Pathfinding<T> Instance { get; private set; }
    
    public Pathfinding(T[,] nodeGrid)
    {
        Instance = this;
        this.nodeGrid = nodeGrid;
    }

    public List<T> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
    {
        CheckedNodeCounter = 0;
        T startNode = nodeGrid[startPosX, startPosY];
        T endNode = nodeGrid[endPosX, endPosY];
        
        List<T> openList = new List<T> { startNode };
        List<T> closedList = new List<T>();
        
        for (int x = 0; x < nodeGrid.GetLength(0); x++)
        {
            for (int y = 0; y < nodeGrid.GetLength(1); y++)
            {
                nodeGrid[x, y].Initialize(x, y);
            }
        }
        
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode.gridPosition, endNode.gridPosition);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            T node = GetLowestFCostNode(openList);
            
            if (node == endNode){
                return CalculatePath(endNode);
            }

            List<T> neighbourList = GetNeighbourList(node);
            
            if (GetNeighbourList(node).Contains(endNode))
            {
                CheckedNodeCounter = closedList.Count;
                return CalculatePath(node);
            }
            
            openList.Remove(node);
            closedList.Add(node);

            foreach (T neighbourNode in neighbourList){
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.IsWalkable){
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = node.gCost + CalculateDistanceCost(node.gridPosition, neighbourNode.gridPosition);
                if (tentativeGCost < neighbourNode.gCost){
                    neighbourNode.cameFromNode = node;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode.gridPosition, endNode.gridPosition);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)){
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        // Out of nodes on the map
        CheckedNodeCounter = closedList.Count;
        return null;
    }
    
    private List <T> GetNeighbourList(T currentNode)
    {
        List<T> neighbourList = new List<T>();

        if (currentNode.gridPosX - 1 >= 0){
            // Left
            neighbourList.Add(nodeGrid[currentNode.gridPosX - 1, currentNode.gridPosY]);
            // Left Down
            if (currentNode.gridPosY - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.gridPosX - 1, currentNode.gridPosY - 1]);
            // Left Up
            if (currentNode.gridPosY + 1 < nodeGrid.GetLength(1)) neighbourList.Add(nodeGrid[currentNode.gridPosX - 1, currentNode.gridPosY + 1]);
        }
        if (currentNode.gridPosX + 1 < nodeGrid.GetLength(0)){
            // Right
            neighbourList.Add(nodeGrid[currentNode.gridPosX + 1, currentNode.gridPosY]);
            // Right Down
            if (currentNode.gridPosY - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.gridPosX + 1, currentNode.gridPosY - 1]);
            // Right Up
            if (currentNode.gridPosY + 1 < nodeGrid.GetLength(1)) neighbourList.Add(nodeGrid[currentNode.gridPosX + 1, currentNode.gridPosY + 1]);
        }
        if (currentNode.gridPosX - 1 >= 0){
            // Down
            if (currentNode.gridPosY - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.gridPosX, currentNode.gridPosY - 1]);
            // Up
            if (currentNode.gridPosY + 1 < nodeGrid.GetLength(1)) neighbourList.Add(nodeGrid[currentNode.gridPosX, currentNode.gridPosY + 1]);
        }

        return neighbourList;
    }
    
    private List<T> CalculatePath(T endNode)
    {
        List<T> path = new List<T>();
        
        path.Add(endNode);
        T currentNode = endNode;
        
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode as T);
            currentNode = currentNode.cameFromNode as T;
        }
        
        path.Reverse();
        return path;
    }
    
    // Calculates a rough estimate of the distance between two positions 
    private int CalculateDistanceCost(Tuple<int, int> a, Tuple<int, int> b)
    {
        int xDistance = Math.Abs(a.Item1 - b.Item1);
        int yDistance = Math.Abs(a.Item2 - b.Item2);
        int remaining = Math.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Math.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    
    private T GetLowestFCostNode (List<T> pathNodeList)
    {
        T lowestFCostNode = pathNodeList[0];
        
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
}