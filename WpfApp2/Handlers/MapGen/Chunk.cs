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
using Microsoft.Windows.Themes;

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
        private ConcurrentBag<cube> cubesToRender= new ConcurrentBag<cube>();
        public void GenerateChunk(string chunkID)
        {
            this.chunkID = chunkID.Split(',').Select(int.Parse).ToArray();

            int xOffset = this.chunkID[0] * Globals.ChunkSize;
            int yOffset = this.chunkID[1] * Globals.ChunkSize;

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
            CubesToRender();
        }
        public ConcurrentBag<cube> GetRenderCubes()
        {
            
            return cubesToRender;
        }

        void CubesToRender()
        {
            for (int z = maxHeight; z >=0; z--)
            {
                bool fullPlane = true;
                for (int x = 0; x < Globals.ChunkSize; x++)
                {
                    for (int y =  0; y < Globals.ChunkSize; y++)
                    {
                        cube cube = GetCube(x, y, z);
                        if (cube != null)
                        {
                            ;
                        }
                        if (cube == null)
                        {
                            fullPlane= false;
                            if (GetCube(x + 1, y, z)!=null)
                            {
                                cubesToRender.Add(GetCube(x + 1, y, z));
                            }
                            if (GetCube(x -1, y, z) != null)
                            {
                                cubesToRender.Add(GetCube(x -1, y, z));
                            }
                            if (GetCube(x , y+1, z) != null)
                            {
                                cubesToRender.Add(GetCube(x, y+1, z));
                            }
                            if (GetCube(x, y - 1, z) != null)
                            {
                                cubesToRender.Add(GetCube(x, y - 1, z));
                            }
                            if (GetCube(x, y , z+1) != null)
                            {
                                cubesToRender.Add(GetCube(x, y, z+1));
                            }
                            if (GetCube(x, y, z - 1) != null)
                            {
                                cubesToRender.Add(GetCube(x, y, z - 1));
                            }
                        }
                    }
                }
                if(fullPlane)
                {
                    break;
                }
            }
        }

        public cube GetCube(int x, int y, int z)
        {
            int xOffset = chunkID[0] * Globals.ChunkSize;
            int yOffset = chunkID[1] * Globals.ChunkSize;
            if (xOffset == 0 && yOffset == 0)
            {
                ; 
            }
            cube cube = cubes.AsParallel().FirstOrDefault(cube => cube.coordinates[0] == x + xOffset && cube.coordinates[1] == y + yOffset && cube.coordinates[2] == z);
            return cube;
        }

    }
}
