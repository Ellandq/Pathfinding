﻿namespace Pathfinding;

public class Pathfinding<T> where T : PathNode
{
    // Movement Grid
    protected T[,] nodeGrid;
    
    // Movement Values
    protected const int MOVE_STRAIGHT_COST = 10;
    protected const int MOVE_DIAGONAL_COST = 14;
    
    public static Pathfinding<T> Instance { get; internal set; }
    
    public virtual List<T> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
    {
        Console.WriteLine("Pathfinding algorithm not chosen.");
        return null;
    }
    
    protected List <T> GetNeighbourList(T currentNode)
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
    
    protected List<T> CalculatePath(T endNode)
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
    protected int CalculateDistanceCost(Tuple<int, int> a, Tuple<int, int> b)
    {
        int xDistance = Math.Abs(a.Item1 - b.Item1);
        int yDistance = Math.Abs(a.Item2 - b.Item2);
        int remaining = Math.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Math.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    
    protected T GetLowestFCostNode (List<T> pathNodeList)
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