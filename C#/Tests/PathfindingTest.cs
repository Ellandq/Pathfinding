namespace Pathfinding.Tests;

public class Tests
{
    public class PathNodeType : PathNode {}

    private Delegate SetUp;
    private Random random = new Random();
    
    private const int WALKABLE_RATE = 80;
    private const bool VISUALIZE_PATH = true;

    private int startPosX;
    private int startPosY;
    private int endPosX;
    private int endPosY;
    
    private string saveName;

    #region AStar Tests

    [Test]
    public void PathfindingRandomTest_AStar_50x50()
    {
        SetUp = (PathNodeType[,] nodeGrid) => new AStar<PathNodeType>(nodeGrid);
        saveName = "50x50_AStar";
        MeasurePathfindingPerformance(50);
    }

    [Test]
    public void PathfindingRandomTest_AStar_150x150()
    {
        SetUp = (PathNodeType[,] nodeGrid) => new AStar<PathNodeType>(nodeGrid);
        saveName = "150x150_AStar";
        MeasurePathfindingPerformance(150);
    }

    [Test]
    public void PathfindingRandomTest_AStar_250x250()
    {
        SetUp = (PathNodeType[,] nodeGrid) => new AStar<PathNodeType>(nodeGrid);
        saveName = "250x250_AStar";
        MeasurePathfindingPerformance(250);
    }

    [Test]
    public void PathfindingRandomTest_AStar_500x500()
    {
        SetUp = (PathNodeType[,] nodeGrid) => new AStar<PathNodeType>(nodeGrid);
        saveName = "500x500_AStar";
        MeasurePathfindingPerformance(500);
    }

    #endregion
    
    #region Djikstra Tests

    [Test]
    public void PathfindingRandomTest_Djikstra_50x50()
    {
        SetUp = (PathNodeType[,] nodeGrid) => new Djikstra<PathNodeType>(nodeGrid);
        saveName = "50x50_Djikstra";
        MeasurePathfindingPerformance(50);
    }

    [Test]
    public void PathfindingRandomTest_Djikstra_150x150()
    {
        SetUp = (PathNodeType[,] nodeGrid) => new Djikstra<PathNodeType>(nodeGrid);
        saveName = "150x150_Djikstra";
        MeasurePathfindingPerformance(150);
    }

    [Test]
    public void PathfindingRandomTest_Djikstra_250x250()
    {
        SetUp = (PathNodeType[,] nodeGrid) => new Djikstra<PathNodeType>(nodeGrid);
        saveName = "250x250_Djikstra";
        MeasurePathfindingPerformance(250);
    }

    #endregion

    private void MeasurePathfindingPerformance(int gridSize)
    {
        Console.WriteLine("First Path:");
        PathNodeType[,] nodeGrid = new PathNodeType[gridSize, gridSize];
        
        do
        {
            startPosX = random.Next(0, gridSize);
            startPosY = random.Next(0, gridSize);
            endPosX = random.Next(0, gridSize);
            endPosY = random.Next(0, gridSize);
        } while ((startPosX == endPosX && startPosY == endPosY) || Math.Abs(startPosX - endPosX) + Math.Abs(startPosY - endPosY) <= 2);
        
        PopulateGrid(nodeGrid, startPosX, startPosY, endPosX, endPosY);

        SetUp.DynamicInvoke(nodeGrid);
        
        List<PathNodeType> path = null;

        
        Stopwatch stopwatch = Stopwatch.StartNew();
        path = Pathfinding<PathNodeType>.Instance.FindPath(startPosX, startPosY, endPosX, endPosY);
        stopwatch.Stop();
        
        double milliseconds = stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;

        if (path != null)
        {
            foreach (var node in path)
            {
                Assert.IsTrue(node.IsWalkable, "All objects on the path need to be walkable.");
            }

            if (VISUALIZE_PATH)
            {
                PathVisualizer<PathNodeType>.VisualizePath(nodeGrid, path, saveName);
            }
        }
        else
        {
            Console.WriteLine("Path was not found.");
        }

        Console.WriteLine($"Start Position: ({startPosX}, {startPosY})");
        Console.WriteLine($"End Position: ({endPosX}, {endPosY})");
        Console.WriteLine($"Number of Nodes checked: {Pathfinding<PathNodeType>.Instance.CheckedNodeCounter}");
        Console.WriteLine($"Elapsed Time: {milliseconds} ms");
        
        try
        {
            int startPosX_02 = random.Next(0, gridSize);
            int startPosY_02 = random.Next(0, gridSize);
            int endPosX_02 = random.Next(0, gridSize);
            int endPosY_02 = random.Next(0, gridSize);
            
            while ((startPosX == endPosX && startPosY == endPosY) || Math.Abs(startPosX - endPosX) + Math.Abs(startPosY - endPosY) <= 2 || !(nodeGrid[startPosX_02,startPosY_02].IsWalkable &&  nodeGrid[endPosX_02,endPosX_02].IsWalkable))
            {
                startPosX_02 = random.Next(0, gridSize);
                startPosY_02 = random.Next(0, gridSize);
                endPosX_02 = random.Next(0, gridSize);
                endPosY_02 = random.Next(0, gridSize);
            } 
                
            stopwatch = Stopwatch.StartNew();
            List<PathNodeType> secondPath = Pathfinding<PathNodeType>.Instance.FindPath(startPosX_02, startPosY_02, endPosX_02, endPosY_02);
            stopwatch.Stop();
            
            milliseconds = stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            
            Console.WriteLine("\nSecond Path:");

            if (path != null)
            {
                foreach (var node in path)
                {
                    Assert.IsTrue(node.IsWalkable, "All objects on the path need to be walkable.");
                }

                if (VISUALIZE_PATH)
                { 
                    //PathVisualizer.VisualizePath(nodeGrid, secondPath, "SECOND");
                }
            }
            else
            {
                Console.WriteLine("Path was not found.");
            }
            
            Console.WriteLine($"Start Position: ({startPosX_02}, {startPosY_02})");
            Console.WriteLine($"End Position: ({endPosX_02}, {endPosY_02})");
            Console.WriteLine($"Number of Nodes checked: {Pathfinding<PathNodeType>.Instance.CheckedNodeCounter}");
            Console.WriteLine($"Elapsed Time: {milliseconds} ms");
            
            
            Console.WriteLine("\nSame starting values as first path:");
            stopwatch = Stopwatch.StartNew();
            List<PathNodeType> thirdPath = Pathfinding<PathNodeType>.Instance.FindPath(startPosX, startPosY, endPosX, endPosY);
            stopwatch.Stop();

            if (thirdPath != null && path != null)
            {
                if (VISUALIZE_PATH)
                {
                    PathVisualizer<PathNodeType>.VisualizePath(nodeGrid, thirdPath, saveName);
                }
                
                Assert.IsTrue(path.Count == thirdPath.Count, "Paths need to be the same lenngth given the same variables");
                
                int index = 0;
                
                foreach (var node in thirdPath)
                {
                    Assert.IsTrue(node == path[index], "Not all path nodes are the same given the same starting variables.");
                    index++;
                }
            
                milliseconds = stopwatch.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
            }
            else if (thirdPath == null && path == null)
            {
                Assert.Pass("Both paths are null");
            }
            else
            {
                Assert.Fail("Paths are different.");
            }
            
            Console.WriteLine($"Elapsed Time: {milliseconds} ms");
        }
        catch (NullReferenceException e)
        {
            Assert.Fail(e.Message);
        }
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
