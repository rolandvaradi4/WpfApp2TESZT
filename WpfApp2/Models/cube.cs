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
using WpfApp2.Config;
using WpfApp2.Handlers.MapGen;
using WpfApp2.Models.Textures;

namespace WpfApp2.Models
{
    public class cube
    {
        ModelVisual3D visual;
        BitmapImage textureImage = TextureID.Grass;
        MeshGeometry3D mesh;
        int[] coordinates;

        public cube(BitmapImage textureImage,int x, int y, int z)
        {
            coordinates = new int[] { x, y, z };

            this.visual = new ModelVisual3D();
            this.textureImage = textureImage;
            this.mesh = GetMesh();

            // Move the model to new coordinates
            TranslateTransform3D translation = new TranslateTransform3D(x, y, z);
            GeometryModel3D model = new GeometryModel3D(mesh, new DiffuseMaterial(new ImageBrush(textureImage)));
            model.Transform= translation;
            visual.Content= model;
            

        }

        private MeshGeometry3D GetMesh()
        {
            MeshGeometry3D mesh_ = new MeshGeometry3D();
            mesh_.Positions = new Point3DCollection(new Point3D[]
           {
                new Point3D(-1, -1, 1),
                new Point3D(1, -1, 1),
                new Point3D(1, 1, 1),
                new Point3D(-1, 1, 1),
                new Point3D(-1, -1, -1),
                new Point3D(1, -1, -1),
                new Point3D(1, 1, -1),
                new Point3D(-1, 1, -1)
           });
            mesh_.TriangleIndices = new Int32Collection(new int[]
            {
                0, 1, 2, 2, 3, 0,
                1, 5, 6, 6, 2, 1,
                5, 4, 7, 7, 6, 5,
                4, 0, 3, 3, 7, 4,
                3, 2, 6, 6, 7, 3,
                4, 5, 1, 1, 0, 4
            });
            mesh_.TextureCoordinates = new PointCollection(new Point[]
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
            mesh_.Normals = new Vector3DCollection(new Vector3D[]
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
            return mesh_;
        }

        public ModelVisual3D GetCube()
        {
            return visual;
        }
    }
}
