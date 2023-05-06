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

        public map(int numCubes)
        {
            for (int i = 0; i < numCubes; i++)
            {
                var cube = new cube().Cube();
                cube.Transform = new TranslateTransform3D(i, 0, 0);
                _cubes.Add(cube);
            }
        }

        public List<ModelVisual3D> Cubes => _cubes;
    }
}
