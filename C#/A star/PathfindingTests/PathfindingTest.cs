namespace PathfindingTests;

public class Tests
{
    private class PathNodeType : PathNode {}

    private Random random = new Random();
    
    private const int WALKABLE_RATE = 80;
    private const bool SHOW_PATH = false;

    private int startPosX;
    private int startPosY;
    private int endPosX;
    private int endPosY;
    
    [Test]
    public void PathfindingRandomTest_AStar_50x50()
    {
        MeasurePathfindingPerformance(50);
    }

    [Test]
    public void PathfindingRandomTest_AStar_150x150()
    {
        MeasurePathfindingPerformance(150);
    }

    [Test]
    public void PathfindingRandomTest_AStar_250x250()
    {
        MeasurePathfindingPerformance(250);
    }

    [Test]
    public void PathfindingRandomTest_AStar_500x500()
    {
        MeasurePathfindingPerformance(500);
    }
    
    [Test]
    public void PathfindingRandomTest_AStar_1000x1000()
    {
        MeasurePathfindingPerformance(1000);
    }

    private void MeasurePathfindingPerformance(int gridSize)
    {
        PathNodeType[,] nodeGrid = new PathNodeType[gridSize, gridSize];
        AStar<PathNodeType> aStar =  new AStar<PathNodeType>(nodeGrid);

        do
        {
            startPosX = random.Next(0, gridSize);
            startPosY = random.Next(0, gridSize);
            endPosX = random.Next(0, gridSize);
            endPosY = random.Next(0, gridSize);
        } while ((startPosX == endPosX && startPosY == endPosY) || Math.Abs(startPosX - endPosX) + Math.Abs(startPosY - endPosY) <= 2);

        Console.WriteLine($"Start Position: ({startPosX}, {startPosY})");
        Console.WriteLine($"End Position: ({endPosX}, {endPosY})");

        PopulateGrid(nodeGrid, startPosX, startPosY, endPosX, endPosY);
        
        List<PathNodeType> path = null;

        
        Stopwatch stopwatch = Stopwatch.StartNew();
        path = aStar.FindPath(startPosX, startPosY, endPosX, endPosY);
        stopwatch.Stop();
        
        double milliseconds = stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

        Assert.Greater(aStar.CheckedNodeCounter, 4, "There need to be more than 4 checked nodes in all cases.");

        if (path != null)
        {
            foreach (var node in path)
            {
                Assert.IsTrue(node.IsWalkable, "All objects on the path need to be walkable.");
            }
            
            

            if (SHOW_PATH)
            {
                Console.WriteLine("Path:");
                foreach (var node in path)
                {
                    Console.WriteLine($"({node.gridPosX}, {node.gridPosY})");
                }
            }
        }
        else
        {
            Console.WriteLine("Path was not found.");
        }

        Console.WriteLine($"Number of Nodes checked: {aStar.CheckedNodeCounter}");

        Console.WriteLine($"Elapsed Time: {milliseconds} ms");
    }

    
    private void PopulateGrid(PathNodeType[,] nodeGrid, int startPosX, int startPosY, int endPosX, int endPosY)
    {
        for (int x = 0; x < nodeGrid.GetLength(0); x++)
        {
            for (int y = 0; y < nodeGrid.GetLength(1); y++)
            {
                nodeGrid[x, y] = new PathNodeType();
                if ((x == startPosX && y == startPosY) || (x == endPosX && y == endPosY) || random.Next(0, 100) < WALKABLE_RATE)
                {
                    nodeGrid[x, y].IsWalkable = true;
                    continue;
                }
                
                nodeGrid[x, y].IsWalkable = false;
            }
        }
    }
}
