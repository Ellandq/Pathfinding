using System;
using System.Collections.Generic;

namespace Pathfinding.Djikstra
{
    public class Dijkstra<T> : Pathfinding<T> where T : PathNode
    {
        public Dijkstra(T[,] nodeGrid)
        {
            Instance = this;
            this.nodeGrid = nodeGrid;
        }

        public override List<T?> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
        {
            CheckedNodeCounter = 0;
            var startNode = nodeGrid[startPosX, startPosY];
            var endNode = nodeGrid[endPosX, endPosY];
            
            Console.WriteLine($"StartPos: {startNode.gridPosition}, EndPos: {endNode.gridPosition}");

            var openList = new HashSet<T?> { startNode };
            var closedList = new HashSet<T?>();

            for (var x = 0; x < nodeGrid.GetLength(0); x++)
            {
                for (var y = 0; y < nodeGrid.GetLength(1); y++)
                {
                    nodeGrid[x, y].Initialize(x, y);
                }
            }

            startNode.GCost = 0;

            while (openList.Count > 0)
            {
                var node = GetLowestGCostNode(openList);
                node.IsChecked = true;

                if (node == endNode)
                {
                    return CalculatePath(endNode);
                }

                HashSet<T?> neighbourList = GetNeighbourList(node);

                openList.Remove(node);
                closedList.Add(node);

                foreach (var neighbourNode in neighbourList.Where(neighbourNode => !closedList.Contains(neighbourNode)))
                {
                    if (!neighbourNode.IsWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    var tentativeGCost = node.GCost + CalculateDistanceCost(node.gridPosition, neighbourNode.gridPosition);
                    if (tentativeGCost >= neighbourNode.GCost) continue;
                    neighbourNode.cameFromNode = node;
                    neighbourNode.GCost = tentativeGCost;

                    openList.Add(neighbourNode);
                }
            }
            // Out of nodes on the map
            CheckedNodeCounter = closedList.Count;
            return null;
        }

        private T? GetLowestGCostNode(HashSet<T?> pathNodeList)
        {
            T? lowestGCostNode = null;
            foreach (var node in pathNodeList.Where(node => lowestGCostNode == null || node.GCost < lowestGCostNode.GCost))
            {
                lowestGCostNode = node;
            }
            return lowestGCostNode;
        }
    }
}
