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

namespace WpfApp2.Handlers.MapGen
{
    public class map
    {
        private readonly Model3DGroup _cubeInstances = new Model3DGroup();
        

        public map(int numCubesX, int numCubesY,int startnumCubesX,int startnumCubesY, Vector3D lookDirection)
        {
            BitmapImage imagea = TextureID.Grass;
            ImageBrush texture = new ImageBrush(imagea);
            // Create a material for the cube
            var material = new DiffuseMaterial(texture);

            // Create the instance transforms
            var transforms = new List<Transform3D>();
            if (lookDirection.X >= 0.463 && lookDirection.Y>= -0.82)  // +positionX
            {
                for (int x = startnumCubesX ; x < startnumCubesX + numCubesX; x++)
                {
                    for (int y = startnumCubesY + 10 ; y > startnumCubesY - numCubesY; y--)
                    {
                        var meshBuilder = new MeshBuilder();
                        meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                        var geometry = meshBuilder.ToMesh();
                        var cubeVisual = new GeometryModel3D(geometry, material);
                        cubeVisual.Transform = new TranslateTransform3D(x, y, 0);
                        _cubeInstances.Children.Add(cubeVisual);
                    }
                }
            }
            else if (lookDirection.Y <= 0 && (lookDirection.X <=0.57 && lookDirection.X >=-0.65) ) // -positionY
            {
                for (int x = startnumCubesX +10 ; x > startnumCubesX - numCubesX  ; x--)
                {
                    for (int y = startnumCubesY; y > startnumCubesY - numCubesY; y--)
                    {
                        var meshBuilder = new MeshBuilder();
                        meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                        var geometry = meshBuilder.ToMesh();
                        var cubeVisual = new GeometryModel3D(geometry, material);
                        cubeVisual.Transform = new TranslateTransform3D(x, y, 0);
                        _cubeInstances.Children.Add(cubeVisual);
                    }
                }
            }
            else if (lookDirection.X <= 0 && lookDirection.Y <= 0.8 )  // -positionX
            {
                for (int x = startnumCubesX; x > startnumCubesX - numCubesX; x--)
                {
                    for (int y = startnumCubesY -10 ; y < startnumCubesY + numCubesY; y++)
                    {
                        var meshBuilder = new MeshBuilder();
                        meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                        var geometry = meshBuilder.ToMesh();
                        var cubeVisual = new GeometryModel3D(geometry, material);
                        cubeVisual.Transform = new TranslateTransform3D(x, y, 0);
                        _cubeInstances.Children.Add(cubeVisual);
                    }
                }
            }
            else if (lookDirection.Y >= 0 && ( lookDirection.X<=0.5 && lookDirection.X>= -0.6 )) // +positionY
            {
                for (int x = startnumCubesX -10 ; x < startnumCubesX + numCubesX; x++)
                {
                    for (int y = startnumCubesY; y < startnumCubesY + numCubesY; y++)
                    {
                        var meshBuilder = new MeshBuilder();
                        meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
                        var geometry = meshBuilder.ToMesh();
                        var cubeVisual = new GeometryModel3D(geometry, material);
                        cubeVisual.Transform = new TranslateTransform3D(x, y, 0);
                        _cubeInstances.Children.Add(cubeVisual);
                    }
                }
            }
        }

        public Model3DGroup CubeInstances => _cubeInstances;
    }
}