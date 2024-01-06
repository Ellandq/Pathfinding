using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PathfindingTests;

public class PathVisualizer
{
    private static readonly Color BLOCKED = Color.FromArgb(0, 0, 0);      
    private static readonly Color WALKABLE = Color.FromArgb(200, 200, 200);    
    private static readonly Color PATH = Color.FromArgb(0, 188, 0);        
    private static readonly Color START = Color.FromArgb(128, 0, 128);    
    private static readonly Color END = Color.FromArgb(0, 0, 255);
    
    public static void VisualizePath(Tests.PathNodeType[,] grid, List<Tests.PathNodeType> path)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Scaling factor for each node
        int scale = 5;

        // Get the current directory of the application
        string currentDirectory = Environment.CurrentDirectory;

        // Create a path to the Visualization folder in the root directory
        string visualizationFolderPath = Path.Combine(currentDirectory, "Visualization");

        // Create the Visualization folder if it doesn't exist
        if (!Directory.Exists(visualizationFolderPath))
        {
            Directory.CreateDirectory(visualizationFolderPath);
        }

        // Create image
        long ticks = DateTime.Now.Ticks;
        string imagePath = Path.Combine(visualizationFolderPath, $"path_visualization_{ticks}.jpg");

        using (Bitmap bitmap = new Bitmap(cols * scale, rows * scale))
        {
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        Color pixelColor = grid[i, j].IsWalkable ? WALKABLE : BLOCKED;
                        SolidBrush brush = new SolidBrush(pixelColor);
                        g.FillRectangle(brush, j * scale, i * scale, scale, scale);
                    }
                }

                for (int i = 0; i < path.Count; i++)
                {
                    Color pixelColor = i == 0 ? START : i == path.Count - 1 ? END : PATH;
                    SolidBrush brush = new SolidBrush(pixelColor);
                    g.FillRectangle(brush, path[i].gridPosX * scale, path[i].gridPosY * scale, scale, scale);
                    // Console.WriteLine($"({path[i].gridPosX}, {path[i].gridPosY})");
                }
            }

            // Save the image to the Visualization folder
            bitmap.Save(imagePath, ImageFormat.Jpeg);
        }

        Console.WriteLine("Image created successfully!");
    }


}