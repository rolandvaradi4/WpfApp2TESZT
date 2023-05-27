using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using WpfApp2.Models;
using WpfApp2.Models.Textures;

namespace WpfApp2.Handlers.MapGen
{

    public class Chunk
    {
        public readonly int chunkSize = 8;
        private readonly int blockHeight = 8;
        private readonly int maxHeight = 64;
        private readonly Random random = new Random();
        private readonly List<ModelVisual3D> models = new List<ModelVisual3D>();

        public List<ModelVisual3D> GenerateChunk(Point3D position)
        {
            int xStart = (int)(position.X / chunkSize) * chunkSize;
            int yStart = (int)(position.Y / chunkSize) * chunkSize;

            for (int x = xStart; x < xStart + chunkSize; x++)
            {
                for (int y = yStart; y < yStart + chunkSize; y++)
                {
                    int height = (int)((Math.Sin(x / 10.0) + Math.Cos(y / 10.0)) * blockHeight) + maxHeight;
                    for (int z = 0; z < height; z++)
                    {
                        var model = new cube().Cube();
                        model.Transform = new TranslateTransform3D(x, y, z);
                        models.Add(model);
                    }
                }
            }

            return models;
        }

        public List<ModelVisual3D> UpdateChunk(Point3D position, Viewport3D viewport)
        {
            int xStart = (int)(position.X / chunkSize) * chunkSize;
            int yStart = (int)(position.Y / chunkSize) * chunkSize;

            // Remove models from the scene
            foreach (var model in models)
            {
                viewport.Children.Remove(model);
            }
            models.Clear();

            // Generate new models
            return GenerateChunk(position);
        }

    }
}
