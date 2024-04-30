using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace Pathfinding.AStar;

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
        
        var startNode = nodeGrid[startPosX, startPosY];
        var endNode = nodeGrid[endPosX, endPosY];
        
        var openList = new List<T> { startNode };
        var closedList = new List<T>();
        var usedList = new List<T>();
        
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode.gridPosition, endNode.gridPosition);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            var node = GetLowestFCostNode(openList);
            node.IsChecked = true;
            
            if (node == endNode){
                CheckedNodeCounter = closedList.Count;
                var path = CalculatePath(node);
                ReinitalizeGrid(usedList);
                return path;
            }

            var neighbourList = GetNeighbourList(node);
            
            if (GetNeighbourList(node).Contains(endNode))
            {
                CheckedNodeCounter = closedList.Count;
                var path = CalculatePath(node);
                ReinitalizeGrid(usedList);
                return path;
            }
            
            openList.Remove(node);
            closedList.Add(node);

            foreach (var neighbourNode in neighbourList.Where(neighbourNode => !closedList.Contains(neighbourNode)))
            {
                if (!neighbourNode.IsUsable && !usedList.Contains(neighbourNode)) usedList.Add(neighbourNode);
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
                    usedList.Add(neighbourNode);
                }
            }
        }
        // Out of nodes on the map
        CheckedNodeCounter = closedList.Count;
        ReinitalizeGrid(usedList);
        return null;
    }
    
    private T GetLowestFCostNode(List<T> pathNodeList)
    {
        return pathNodeList.Where(node => node.IsWalkable).OrderBy(node => node.fCost).FirstOrDefault();
    }

}
