using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Pathfinding;

namespace Pathfinding.Visualisation;

public class PathVisualizer<T> where T : PathNode
{
    private static readonly Color Blocked = Color.FromArgb(0, 0, 0);      
    private static readonly Color Walkable = Color.FromArgb(200, 200, 200);    
    private static readonly Color Checked = Color.FromArgb(200, 50, 50);    
    private static readonly Color Path = Color.FromArgb(0, 188, 0);        
    private static readonly Color Start = Color.FromArgb(128, 0, 128);    
    private static readonly Color End = Color.FromArgb(0, 0, 255);
    
    public static void VisualizePath(T[,] grid, List<T> path, string msg = "")
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);
        
        Console.WriteLine(path[0].gridPosition + " " + path[^1].gridPosition);

        // Scaling factor for each node
        const int scale = 10;
        
        var currentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName;

        var projectDirectory = System.IO.Path.Combine(currentDirectory, "Visualisations");

        if (!Directory.Exists(projectDirectory))
        {
            Directory.CreateDirectory(projectDirectory);
            Directory.CreateDirectory(System.IO.Path.Combine(projectDirectory, "50x50"));
            Directory.CreateDirectory(System.IO.Path.Combine(projectDirectory, "150x150"));
            Directory.CreateDirectory(System.IO.Path.Combine(projectDirectory, "250x250"));
            Directory.CreateDirectory(System.IO.Path.Combine(projectDirectory, "500x500"));
        }
        
        var ticks = DateTime.Now.Ticks;
        var size = grid.GetLength(0);
        var imagePath = System.IO.Path.Combine(projectDirectory, $"{size}x{size}", $"C#_path_visualization_{msg}_{ticks}.jpg");
        
        using (var bitmap = new Bitmap(cols * scale, rows * scale))
        {
            using (var g = Graphics.FromImage(bitmap))
            {
                for (var i = 0; i < rows; i++)
                {
                    for (var j = 0; j < cols; j++)
                    {
                        var pixelColor = grid[j, i].IsWalkable ? grid[j, i].IsChecked ? Checked : Walkable : Blocked;
                        var brush = new SolidBrush(pixelColor);
                        grid[j, i].IsChecked = false;
                        g.FillRectangle(brush, j * scale, i * scale, scale, scale);
                    }
                }

                for (var i = 0; i < path.Count; i++)
                {
                    var pixelColor = i == 0 ? Start : i == path.Count - 1 ? End : Path;
                    var brush = new SolidBrush(pixelColor);
                    g.FillRectangle(brush, path[i].GridPosX * scale, path[i].GridPosY * scale, scale, scale);
                }
            }
            
            bitmap.Save(imagePath, ImageFormat.Jpeg);
        }

        Console.WriteLine("Image created successfully!");
    }


}