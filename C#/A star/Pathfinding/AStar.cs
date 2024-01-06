using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace Pathfinding;

public class AStar<T> : Pathfinding<T> where T : PathNode
{
    public AStar(T[,] nodeGrid)
    {
        Instance = this;
        InitializeNodes(nodeGrid);
    }

    public override List<T> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
    {
        CheckedNodeCounter = 0;
        T startNode = nodeGrid[startPosX, startPosY];
        T endNode = nodeGrid[endPosX, endPosY];
        
        List<T> openList = new List<T> { startNode };
        List<T> closedList = new List<T>();
        
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

            foreach (T neighbourNode in neighbourList)
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.IsUsable) neighbourNode.Initialize();
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

                    neighbourNode.IsUsable = false;
                }
            }
        }
        // Out of nodes on the map
        CheckedNodeCounter = closedList.Count;
        return null;
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
