using System.Runtime.InteropServices;

namespace Pathfinding;

public class PathNode
{
    public PathNode? cameFromNode = null; // Reference to the previous node in the search

    public Tuple<int, int> gridPosition { get; private set;  }

    public int GridPosX { get { return gridPosition.Item1; } }
    public int GridPosY { get { return gridPosition.Item2; } }
    
    public int GCost = 999999; // Walking cost from start node
    public int HCost = 0; // Heuristic cost to reach end node
    public int FCost = 0; // G cost + H cost
    
    // A public property to set if a given Node should be accessible
    public bool IsWalkable { get; set; } = true;
    public bool IsUsable { get; set; } = true;
    public bool IsChecked { get; set; } = false;

    public void Initialize(int x, int y)
    {
        gridPosition = new Tuple<int, int>(x, y);
        Initialize();
    }
    
    public void Initialize()
    {
        GCost = 999999;
        cameFromNode = null;
        CalculateFCost();
        IsUsable = true;
    }
    
    public void CalculateFCost()
    {
        FCost = GCost + HCost;
    }

    public static bool operator ==(PathNode? a, PathNode? b)
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

    public static bool operator !=(PathNode? a, PathNode b)
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