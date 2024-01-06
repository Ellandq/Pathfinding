package Pathfinding;

import Utils.Pair;

public class PathNode
{
    public PathNode cameFromNode = null;

    public Pair<Integer, Integer> gridPosition;

    public int gCost = 999999;
    public int hCost = 0;
    public int fCost = 0;

    public boolean walkable = true;
    public boolean usable = true;

    public void initialize (int x, int y) {
        gridPosition = new Pair<>(x, y);
        initialize();
    }

    public void initialize (){
        gCost = 999999;
        cameFromNode = null;
        usable = true;
        CalculateFCost();
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public Pair<Integer, Integer> getGridPosition() {
        return gridPosition;
    }

    public Integer getGridPosX () {
        return gridPosition.value1;
    }

    public Integer getGridPosY () {
        return gridPosition.value2;
    }

    @Override
    public boolean equals(Object obj) {
        if (this == obj) {
            return true;
        }
        if (obj == null || getClass() != obj.getClass()) {
            return false;
        }
        PathNode other = (PathNode) obj;
        return Pair.equals(gridPosition, other.gridPosition);
    }

    public static boolean equals(PathNode a, PathNode b) {
        if (a == null && b == null) {
            return true;
        }
        if (a == null || b == null) {
            return false;
        }
        return a.equals(b);
    }

    public static boolean notEquals(PathNode a, PathNode b) {
        return !equals(a, b);
    }
}
