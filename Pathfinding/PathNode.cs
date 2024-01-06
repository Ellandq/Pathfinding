using System.Runtime.InteropServices;

namespace Pathfinding;

public class PathNode
{
    public PathNode cameFromNode; // Reference to the previous node in the search

    public Tuple<int, int> gridPosition { get; private set;  }

    public int gridPosX { get { return gridPosition.Item1; } }
    public int gridPosY { get { return gridPosition.Item2; } }
    
    public int gCost = 0; // Walking cost from start node
    public int hCost = 0; // Heuristic cost to reach end node
    public int fCost = 0; // G cost + H cost
    
    // A public property to set if a given Node should be accessible
    public bool IsWalkable { get; set; }

    public void Initialize(int x, int y)
    {
        gridPosition = new Tuple<int, int>(x, y);
        gCost = 999999;
        cameFromNode = null;
        CalculateFCost();
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public static bool operator ==(PathNode a, PathNode b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return true;
        }
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return false;
        }

        return a.gridPosition.Equals(b.gridPosition);
    }

    public static bool operator !=(PathNode a, PathNode b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        {
            return false;
        }
        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        {
            return true;
        }

        return !a.gridPosition.Equals(b.gridPosition);
    }

}