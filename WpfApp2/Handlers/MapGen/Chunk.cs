using Accord.Math.Random;
using Accord.MachineLearning;
using Accord.MachineLearning;

using HelixToolkit.Wpf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using WpfApp2.Config;
using WpfApp2.Models;
using WpfApp2.Models.Textures;
using Accord.Math;

namespace WpfApp2.Handlers.MapGen
{

    public class Chunk
    {
        public int[] chunkID { get; set; }
        bool isLoaded { get; set; }
        
        
        private readonly int blockHeight = 8;
        private int maxHeight = 0;
        private readonly Random random = new Random();
        private ConcurrentBag<cube> cubes= new ConcurrentBag<cube>();

        public void GenerateChunk(string chunkID)
        {
            this.chunkID = chunkID.Split(',').Select(int.Parse).ToArray();

            int xOffset = chunkID[0] * Globals.ChunkSize;
            int yOffset = chunkID[1] * Globals.ChunkSize;

            Parallel.For(xOffset, xOffset + Globals.ChunkSize, x =>
            {
                Parallel.For(yOffset, yOffset + Globals.ChunkSize, y =>
                {
                    int maxHeight = 64; // Set the initial maximum height

                    // Use Accord.NET's PerlinNoise class for noise generation
                    PerlinNoise noise = new PerlinNoise();
                    double noiseScale = 0.1; // Adjust this value to control the smoothness
                    double noiseOffset = 0.5; // Adjust this value to control the overall height

                    int height = (int)(maxHeight * (noise.Function2D(x * noiseScale, y * noiseScale) + noiseOffset));

                    for (int z = 0; z < height; z++)
                    {
                        var cube = new cube(new BitmapImage(new Uri("pack://application:,,,/Models/Textures/grass.png", UriKind.RelativeOrAbsolute)), x, y, z);
                        cubes.Add(cube);
                        if(z>this.maxHeight)
                        {
                            this.maxHeight = z;
                        }
                    }
                });
            });
        }

        public List<ModelVisual3D> UpdateChunk(Point3D position, Viewport3D viewport)
        {
            return null;
        }

    }
}
