using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WpfApp2
{
    public class cube
    {
        public ModelVisual3D Cube()
        {
            // Create the cube mesh
            BitmapImage textureImage = new BitmapImage(new Uri("D:\\4.feleves\\wpf\\teszt2\\WpfApp2TESZT\\WpfApp2\\wildgrass_2_ur_1024.png", UriKind.Relative));
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions = new Point3DCollection(new Point3D[]
            {
                new Point3D(-0.5, -0.5, 0.5),
                new Point3D(0.5, -0.5, 0.5),
                new Point3D(0.5, 0.5, 0.5),
                new Point3D(-0.5, 0.5, 0.5),
                new Point3D(-0.5, -0.5, -0.5),
                new Point3D(0.5, -0.5, -0.5),
                new Point3D(0.5, 0.5, -0.5),
                new Point3D(-0.5, 0.5, -0.5)
            });
            mesh.TriangleIndices = new Int32Collection(new int[]
            {
                0, 1, 2, 2, 3, 0,
                1, 5, 6, 6, 2, 1,
                5, 4, 7, 7, 6, 5,
                4, 0, 3, 3, 7, 4,
                3, 2, 6, 6, 7, 3,
                4, 5, 1, 1, 0, 4
            });
            mesh.TextureCoordinates = new PointCollection(new Point[]
            {
                    // Front
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1),

                // Back
                new Point(1, 1),
                new Point(0, 1),
                new Point(0, 0),
                new Point(1, 0),

                // Top
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(0, 1),

                // Bottom
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                new Point(0, 0),

                // Left
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                new Point(0, 0),

                // Right
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                new Point(0, 0)

                     });


            mesh.Normals = new Vector3DCollection(new Vector3D[]
            {
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, 1),
                new Vector3D(0, 0, -1),
                new Vector3D(0, 0, -1),
                new Vector3D(0, 0, -1),
                new Vector3D(0, 0, -1)
            });
            GeometryModel3D model = new GeometryModel3D(mesh, new DiffuseMaterial(new ImageBrush(textureImage)));






            // Create the cube visual
            DiffuseMaterial material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            ModelVisual3D visual = new ModelVisual3D();
            visual.Content = model;
            return visual;
        }
    }
}
