using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using HelixToolkit.Wpf;
using System.Windows.Media.Imaging;
using WpfApp2.Models.Textures;
using System.Collections.Concurrent;

namespace WpfApp2.Handlers.MapGen
{
    public class Map
    {
        public ConcurrentBag<Chunk> Chunks = new ConcurrentBag<Chunk>();

        public Map()
        {
            Parallel.For(-2, 2, x =>
            {
                Parallel.For(-2, 2, y =>
                {
                    Chunk chunk = new Chunk();
                    chunk.GenerateChunk($"{x},{y}");

                    Chunks.Add(chunk);
                });
            });
        }


        public async void GenerateChunkByCoordinates(int x, int y)
        {
            Chunk chunk = new Chunk();
            chunk.GenerateChunk($"{x},{y}");
            Chunks.Add(chunk);
        }

        public Chunk GetChunkByCoordinates(int x, int y)
        {
            foreach (Chunk chunk in Chunks)
            {
                int[] coordinates = chunk.chunkID;
                if (coordinates[0] == x && coordinates[1] == y)
                {
                    return chunk;
                }
            }
             GenerateChunkByCoordinates(x, y); // Chunk not found
            return GetChunkByCoordinates(x, y);
        }

    }
}