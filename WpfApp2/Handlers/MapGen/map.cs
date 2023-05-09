using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using HelixToolkit.Wpf;

namespace WpfApp2.Handlers.MapGen
{
    public class map
    {
        private readonly Model3DGroup _cubeInstances = new Model3DGroup();

        public map(int numCubesX, int numCubesZ, ImageBrush texture)
        {
            // Create a material for the cube
            var material = new DiffuseMaterial(texture);

            // Create the instance transforms
            var transforms = new List<Transform3D>();
            for (int x = 0; x < numCubesX; x++)
            {
                for (int z = 0; z < numCubesZ; z++)
                {
                    var meshBuilder = new MeshBuilder();
                    meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                    var geometry = meshBuilder.ToMesh();
                    var cubeVisual = new GeometryModel3D(geometry, material);
                    cubeVisual.Transform = new TranslateTransform3D(x, 0, z);
                    _cubeInstances.Children.Add(cubeVisual);
                }
            }
        }

        public Model3DGroup CubeInstances => _cubeInstances;
    }
}
