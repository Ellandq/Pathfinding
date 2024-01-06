namespace PathfindingTests;

public class Tests
{
    private class PathNodeType : PathNode {}

    private Random random = new Random();
    
    private const int WALKABLE_RATE = 70;
    private const bool SHOW_PATH = false;

    private int startPosX;
    private int startPosY;
    private int endPosX;
    private int endPosY;
    
    [Test]
    public void PathfindingRandomTest_50x50()
    {
        MeasurePathfindingPerformance(50);
    }

    [Test]
    public void PathfindingRandomTest_150x150()
    {
        MeasurePathfindingPerformance(150);
    }

    [Test]
    public void PathfindingRandomTest_250x250()
    {
        MeasurePathfindingPerformance(250);
    }

    [Test]
    public void PathfindingRandomTest_500x500()
    {
        MeasurePathfindingPerformance(500);
    }
    
    [Test]
    public void PathfindingRandomTest_1000x1000()
    {
        MeasurePathfindingPerformance(1000);
    }

    private void MeasurePathfindingPerformance(int gridSize)
    {
        PathNodeType[,] nodeGrid = new PathNodeType[gridSize, gridSize];
        Pathfinding<PathNodeType> pathfinding =  new Pathfinding<PathNodeType>(nodeGrid);

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

        List<long> elapsedTimes = new List<long>();
        List<PathNodeType> path = null;

        for (int i = 0; i < 10; i++) // Run the test 10 times
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            path = pathfinding.FindPath(startPosX, startPosY, endPosX, endPosY);
            stopwatch.Stop();

            elapsedTimes.Add(stopwatch.ElapsedTicks);
        }

        double averageMilliseconds = elapsedTimes.Average() * 1000.0 / Stopwatch.Frequency;

        Assert.Greater(pathfinding.CheckedNodeCounter, 4, "There need to be more than 4 checked nodes in all cases.");

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

        Console.WriteLine($"Number of Nodes checked: {pathfinding.CheckedNodeCounter}");

        Console.WriteLine($"Average Elapsed Time: {averageMilliseconds} ms");
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
