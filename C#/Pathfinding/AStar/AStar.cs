using System;
using System.Collections.Generic;

namespace Pathfinding.AStar
{
    public class AStar<T> : Pathfinding<T> where T : PathNode
    {
        public AStar(T[,] nodeGrid)
        {
            Instance = this;
            InitializeNodes(nodeGrid);
        }

        public override List<T?> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
        {
            CheckedNodeCounter = 0;

            var startNode = nodeGrid[startPosX, startPosY];
            var endNode = nodeGrid[endPosX, endPosY];

            var openList = new HashSet<T?> { startNode };
            var closedList = new HashSet<T?>();
            var usedList = new HashSet<T?>();

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode.gridPosition, endNode.gridPosition);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                var node = GetLowestFCostNode(openList);
                node.IsChecked = true;

                if (node == endNode)
                {
                    CheckedNodeCounter = closedList.Count;
                    var path = CalculatePath(node);
                    ReinitalizeGrid(usedList);
                    return path;
                }

                var neighbourList = GetNeighbourList(node);

                openList.Remove(node);
                closedList.Add(node);

                foreach (var neighbourNode in neighbourList.Where(neighbourNode => !closedList.Contains(neighbourNode)))
                {
                    if (!neighbourNode.IsUsable)
                        usedList.Add(neighbourNode);
                    if (!neighbourNode.IsWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    var tentativeGCost = node.GCost + CalculateDistanceCost(node.gridPosition, neighbourNode.gridPosition);
                    if (tentativeGCost >= neighbourNode.GCost) continue;
                    neighbourNode.cameFromNode = node;
                    neighbourNode.GCost = tentativeGCost;
                    neighbourNode.HCost = CalculateDistanceCost(neighbourNode.gridPosition, endNode.gridPosition);
                    neighbourNode.CalculateFCost();

                    openList.Add(neighbourNode);

                    neighbourNode.IsUsable = false;
                    usedList.Add(neighbourNode);
                }
            }
            // Out of nodes on the map
            CheckedNodeCounter = closedList.Count;
            ReinitalizeGrid(usedList);
            return null;
        }

        private T? GetLowestFCostNode(HashSet<T?> pathNodeList)
        {
            T? lowestCostNode = null;
            foreach (var node in pathNodeList.Where(node => node.IsWalkable && (lowestCostNode == null || node.FCost < lowestCostNode.FCost)))
            {
                lowestCostNode = node;
            }

            return lowestCostNode;
        }
    }
}
