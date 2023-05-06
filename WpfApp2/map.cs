using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WpfApp2
{
    public class map
    {
        private readonly List<ModelVisual3D> _cubes = new List<ModelVisual3D>();

        public map(int sizeX, int sizeY, int sizeZ)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        var cube = new cube().Cube();
                        cube.Transform = new TranslateTransform3D(x, y, z);
                        _cubes.Add(cube);
                    }
                }
            }
        }

        public List<ModelVisual3D> Cubes => _cubes;
    }
}
