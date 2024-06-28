using System;
using System.Collections.Generic;

// Full repository can be found on : https://github.com/Ellandq/Pathfinding/tree/main/C%23/Pathfinding

namespace Pathfinding
{
    public class Pathfinding<T> where T : PathNode
    {
        // Movement Grid
        protected T[,] nodeGrid;

        // Movement Values
        protected const int MOVE_STRAIGHT_COST = 10;
        protected const int MOVE_DIAGONAL_COST = 14;

        public static Pathfinding<T> Instance { get; protected set; }

        public int CheckedNodeCounter { get; protected set; }

        public virtual List<T?> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
        {
            Console.WriteLine("Pathfinding algorithm not chosen.");
            return null;
        }

        protected void InitializeNodes(T[,] nodeGrid)
        {
            this.nodeGrid = nodeGrid;

            for (var x = 0; x < nodeGrid.GetLength(0); x++)
            {
                for (var y = 0; y < nodeGrid.GetLength(1); y++)
                {
                    this.nodeGrid[x, y].Initialize(x, y);
                }
            }
        }

        protected virtual HashSet<T> GetNeighbourList(T currentNode)
        {
            var neighbourList = new HashSet<T>();

            if (currentNode.GridPosX - 1 >= 0)
            {
                // Left
                neighbourList.Add(nodeGrid[currentNode.GridPosX - 1, currentNode.GridPosY]);
                // Left Down
                if (currentNode.GridPosY - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.GridPosX - 1, currentNode.GridPosY - 1]);
                // Left Up
                if (currentNode.GridPosY + 1 < nodeGrid.GetLength(1)) neighbourList.Add(nodeGrid[currentNode.GridPosX - 1, currentNode.GridPosY + 1]);
            }
            if (currentNode.GridPosX + 1 < nodeGrid.GetLength(0))
            {
                // Right
                neighbourList.Add(nodeGrid[currentNode.GridPosX + 1, currentNode.GridPosY]);
                // Right Down
                if (currentNode.GridPosY - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.GridPosX + 1, currentNode.GridPosY - 1]);
                // Right Up
                if (currentNode.GridPosY + 1 < nodeGrid.GetLength(1)) neighbourList.Add(nodeGrid[currentNode.GridPosX + 1, currentNode.GridPosY + 1]);
            }
            if (currentNode.GridPosX - 1 >= 0)
            {
                // Down
                if (currentNode.GridPosY - 1 >= 0) neighbourList.Add(nodeGrid[currentNode.GridPosX, currentNode.GridPosY - 1]);
                // Up
                if (currentNode.GridPosY + 1 < nodeGrid.GetLength(1)) neighbourList.Add(nodeGrid[currentNode.GridPosX, currentNode.GridPosY + 1]);
            }

            return neighbourList;
        }

        protected List<T?> CalculatePath(T? endNode)
        {
            var path = new List<T?> { endNode };

            var currentNode = endNode;

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
            var xDistance = Math.Abs(a.Item1 - b.Item1);
            var yDistance = Math.Abs(a.Item2 - b.Item2);
            var remaining = Math.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Math.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        protected void ReinitalizeGrid(HashSet<T> used)
        {
            foreach (var node in used)
            {
                node.Initialize();
            }
        }
    }
}
