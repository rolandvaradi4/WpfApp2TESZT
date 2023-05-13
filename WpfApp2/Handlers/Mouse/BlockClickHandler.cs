using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp2.Handlers.Camera;
using WpfApp2.Handlers.MapGen;
using WpfApp2.Models.Textures;

namespace WpfApp2.Handlers.Mouse
{
    public class BlockClickHandler
    {
        Viewport3D viewport;
        CameraHandlers camera;
        MapChunk map;
        public readonly Model3DGroup CubeBlocks = new Model3DGroup();

        public BlockClickHandler(Viewport3D viewport, CameraHandlers camera, MapChunk map)
        {
            this.viewport = viewport; 
            this.camera = camera;
            this.map = map;
        }

        public void AddBlock(Viewport3D viewport, CameraHandlers camera, MapChunk map)
        {
            Point3D Position = camera.playerCamera.Position;
            Point3D Target = camera.playerCamera.LookDirection + camera.playerCamera.Position;

            BitmapImage imagea = TextureID.Grass;
            ImageBrush texture = new ImageBrush(imagea);
            // Create a material for the cube
            var material = new DiffuseMaterial(texture);
            int x = (int)Math.Round(Target.X);
            int y = (int)Math.Round(Target.Y);
            int z = (int)Math.Round(Target.Z);
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
            var geometry = meshBuilder.ToMesh();
            var cubeVisual = new GeometryModel3D(geometry, material);
            cubeVisual.Transform = new TranslateTransform3D(x, y, z);
            CubeBlocks.Children.Add(cubeVisual);
            

        }

        public void RemoveBlock(Viewport3D viewport, CameraHandlers camera, MapChunk map)
        {
            Point3D Position = camera.playerCamera.Position;
            Point3D Target = camera.playerCamera.LookDirection + camera.playerCamera.Position;

            BitmapImage imagea = TextureID.Grass;
            ImageBrush texture = new ImageBrush(imagea);
            // Create a material for the cube
            var material = new DiffuseMaterial(texture);

            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
            var geometry = meshBuilder.ToMesh();
            var cubeVisual = new GeometryModel3D(geometry, material);
            cubeVisual.Transform = new TranslateTransform3D(Target.X, Target.Y, Target.Z);
            map.CubeInstances.Children.Remove(cubeVisual);

        }
    }
}
