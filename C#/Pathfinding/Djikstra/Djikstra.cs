using System;
using System.Collections.Generic;

namespace Pathfinding.Djikstra
{
    public class Djikstra<T> : Pathfinding<T> where T : PathNode
    {
        public Djikstra(T[,] nodeGrid)
        {
            Instance = this;
            this.nodeGrid = nodeGrid;
        }

        public override List<T> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
        {
            CheckedNodeCounter = 0;
            var startNode = nodeGrid[startPosX, startPosY];
            var endNode = nodeGrid[endPosX, endPosY];

            var openList = new HashSet<T> { startNode };
            var closedList = new HashSet<T>();

            for (var x = 0; x < nodeGrid.GetLength(0); x++)
            {
                for (var y = 0; y < nodeGrid.GetLength(1); y++)
                {
                    nodeGrid[x, y].Initialize(x, y);
                }
            }

            startNode.gCost = 0;

            while (openList.Count > 0)
            {
                var node = GetLowestGCostNode(openList);
                node.IsChecked = true;

                if (node == endNode)
                {
                    return CalculatePath(endNode);
                }

                var neighbourList = GetNeighbourList(node);

                if (GetNeighbourList(node).Contains(endNode))
                {
                    CheckedNodeCounter = closedList.Count;
                    return CalculatePath(node);
                }

                openList.Remove(node);
                closedList.Add(node);

                foreach (var neighbourNode in neighbourList)
                {
                    if (!closedList.Contains(neighbourNode))
                    {
                        if (!neighbourNode.IsWalkable)
                        {
                            closedList.Add(neighbourNode);
                            continue;
                        }

                        var tentativeGCost = node.gCost + CalculateDistanceCost(node.gridPosition, neighbourNode.gridPosition);
                        if (tentativeGCost >= neighbourNode.gCost) continue;
                        neighbourNode.cameFromNode = node;
                        neighbourNode.gCost = tentativeGCost;

                        openList.Add(neighbourNode);
                    }
                }
            }
            // Out of nodes on the map
            CheckedNodeCounter = closedList.Count;
            return null;
        }

        private T GetLowestGCostNode(HashSet<T> pathNodeList)
        {
            T lowestGCostNode = null;
            foreach (var node in pathNodeList)
            {
                if (lowestGCostNode == null || node.gCost < lowestGCostNode.gCost)
                {
                    lowestGCostNode = node;
                }
            }
            return lowestGCostNode;
        }
    }
}
