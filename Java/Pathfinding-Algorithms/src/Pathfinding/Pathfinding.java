package Pathfinding;

import java.util.ArrayList;
import java.util.List;
import Utils.Pair;

public class Pathfinding<T extends PathNode> {

    // Movement Grid
    protected T[][] nodeGrid;

    // Movement Values
    protected final int MOVE_STRAIGHT_COST = 10;
    protected final int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding<PathNode> Instance; // Java does not have static generic types, so using PathNode for the example

    public int CheckedNodeCounter;

    public List<T> findPath(int startPosX, int startPosY, int endPosX, int endPosY) {
        System.out.println("Pathfinding algorithm not chosen.");
        return null;
    }

    protected void initializeNodes(T[][] nodeGrid) {
        this.nodeGrid = nodeGrid;

        for (int x = 0; x < nodeGrid.length; x++) {
            for (int y = 0; y < nodeGrid[x].length; y++) {
                this.nodeGrid[x][y].initialize(x, y);
            }
        }
    }

    protected List<T> getNeighbourList(T currentNode) {
        List<T> neighbourList = new ArrayList<>();

        if (currentNode.getGridPosX() - 1 >= 0) {
            // Left
            neighbourList.add(nodeGrid[currentNode.getGridPosX() - 1][currentNode.getGridPosY()]);
            // Left Down
            if (currentNode.getGridPosY() - 1 >= 0) neighbourList.add(nodeGrid[currentNode.getGridPosX() - 1][currentNode.getGridPosY() - 1]);
            // Left Up
            if (currentNode.getGridPosY() + 1 < nodeGrid[0].length) neighbourList.add(nodeGrid[currentNode.getGridPosX() - 1][currentNode.getGridPosY() + 1]);
        }
        if (currentNode.getGridPosX() + 1 < nodeGrid.length) {
            // Right
            neighbourList.add(nodeGrid[currentNode.getGridPosX() + 1][currentNode.getGridPosY()]);
            // Right Down
            if (currentNode.getGridPosY() - 1 >= 0) neighbourList.add(nodeGrid[currentNode.getGridPosX() + 1][currentNode.getGridPosY() - 1]);
            // Right Up
            if (currentNode.getGridPosY() + 1 < nodeGrid[0].length) neighbourList.add(nodeGrid[currentNode.getGridPosX() + 1][currentNode.getGridPosY() + 1]);
        }
        if (currentNode.getGridPosX() - 1 >= 0) {
            // Down
            if (currentNode.getGridPosY() - 1 >= 0) neighbourList.add(nodeGrid[currentNode.getGridPosX()][currentNode.getGridPosY() - 1]);
            // Up
            if (currentNode.getGridPosY() + 1 < nodeGrid[0].length) neighbourList.add(nodeGrid[currentNode.getGridPosX()][currentNode.getGridPosY() + 1]);
        }

        return neighbourList;
    }

    protected List<T> calculatePath(T endNode) {
        List<T> path = new ArrayList<>();

        path.add(endNode);
        T currentNode = endNode;

        while (currentNode.cameFromNode != null) {
            path.add((T) currentNode.cameFromNode);
            currentNode = (T) currentNode.cameFromNode;
        }

        java.util.Collections.reverse(path);
        return path;
    }

    protected int calculateDistanceCost(Pair<Integer, Integer> a, Pair<Integer, Integer> b) {
        int xDistance = Math.abs(a.value1 - b.value1);
        int yDistance = Math.abs(a.value2 - b.value2);
        int remaining = Math.abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Math.min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    protected void reinitalizeGrid(List<T> used) {
        for (T node : used) {
            node.initialize();
        }
    }
}

