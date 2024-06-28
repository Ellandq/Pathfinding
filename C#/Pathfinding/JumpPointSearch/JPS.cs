namespace Pathfinding.JumpPointSearch;

public class JPS<T> : Pathfinding<T> where T : PathNode
{
    // TODO

    public override List<T?> FindPath(int startPosX, int startPosY, int endPosX, int endPosY)
    {
        // TODO
        return base.FindPath(startPosX, startPosY, endPosX, endPosY);
    }

    protected override HashSet<T> GetNeighbourList(T currentNode)
    {
        // TODO
        return base.GetNeighbourList(currentNode);
    }

    private T Jump(T currentNode, T parent, Tuple<int, int> direction)
    {
        // TODO
        return null;
    }
    
    private bool IsWalkable(int x, int y)
    {
        return IsWithinBounds(x, y) && nodeGrid[x, y].IsWalkable;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < nodeGrid.GetLength(0) && y >= 0 && y < nodeGrid.GetLength(1);
    }

    private List<Tuple<int, int>> GetPossibleDirections()
    {
        return new List<Tuple<int, int>>
        {
            new Tuple<int, int>(1, 0),   // Right
            new Tuple<int, int>(-1, 0),  // Left
            new Tuple<int, int>(0, 1),   // Up
            new Tuple<int, int>(0, -1),  // Down
            new Tuple<int, int>(1, 1),   // Diagonal Up-Right
            new Tuple<int, int>(-1, 1),  // Diagonal Up-Left
            new Tuple<int, int>(1, -1),  // Diagonal Down-Right
            new Tuple<int, int>(-1, -1)  // Diagonal Down-Left
        };
    }

}