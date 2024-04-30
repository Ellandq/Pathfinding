using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Pathfinding;

namespace Pathfinding.Visualisation;

public class PathVisualizer<T> where T : PathNode
{
    private static readonly Color BLOCKED = Color.FromArgb(0, 0, 0);      
    private static readonly Color WALKABLE = Color.FromArgb(200, 200, 200);    
    private static readonly Color CHECKED = Color.FromArgb(200, 50, 50);    
    private static readonly Color PATH = Color.FromArgb(0, 188, 0);        
    private static readonly Color START = Color.FromArgb(128, 0, 128);    
    private static readonly Color END = Color.FromArgb(0, 0, 255);
    
    public static void VisualizePath(T[,] grid, List<T> path, string msg = "")
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Scaling factor for each node
        int scale = 10;
        
        string currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;

        string projectDirectory = Path.Combine(currentDirectory, "Visualisations");

        if (!Directory.Exists(projectDirectory))
        {
            Directory.CreateDirectory(projectDirectory);
            Directory.CreateDirectory(Path.Combine(projectDirectory, "50x50"));
            Directory.CreateDirectory(Path.Combine(projectDirectory, "150x150"));
            Directory.CreateDirectory(Path.Combine(projectDirectory, "250x250"));
            Directory.CreateDirectory(Path.Combine(projectDirectory, "500x500"));
        }
        
        long ticks = DateTime.Now.Ticks;
        int size = grid.GetLength(0);
        string imagePath = Path.Combine(projectDirectory, $"{size}x{size}", $"C#_path_visualization_{msg}_{ticks}.jpg");
        
        using (var bitmap = new Bitmap(cols * scale, rows * scale))
        {
            using (var g = Graphics.FromImage(bitmap))
            {
                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < cols; j++)
                    {
                        var pixelColor = grid[j, i].IsWalkable ? grid[j, i].IsChecked ? CHECKED : WALKABLE : BLOCKED;
                        var brush = new SolidBrush(pixelColor);
                        grid[j, i].IsChecked = false;
                        g.FillRectangle(brush, j * scale, i * scale, scale, scale);
                    }
                }

                for (int i = 0; i < path.Count; i++)
                {
                    Color pixelColor = i == 0 ? START : i == path.Count - 1 ? END : PATH;
                    SolidBrush brush = new SolidBrush(pixelColor);
                    g.FillRectangle(brush, path[i].gridPosX * scale, path[i].gridPosY * scale, scale, scale);
                }
            }
            
            bitmap.Save(imagePath, ImageFormat.Jpeg);
        }

        Console.WriteLine("Image created successfully!");
    }


}