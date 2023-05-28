using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using WpfApp2.Models.Textures;

namespace WpfApp2.Models
{
    internal class Tree
    {
        private readonly Model3DGroup Tree3dmodel = new Model3DGroup();

        public void MakeTreemodel(int x, int y)
        {
            BitmapImage imagea = TextureID.Tree;
            ImageBrush texture = new ImageBrush(imagea);
            BitmapImage imagea2 = TextureID.Leaf;
            ImageBrush texture2 = new ImageBrush(imagea2);

            int height = 0;
            Enumerable.Range(0, 4).ToList().ForEach(i =>
            {
                AddCube(new Point3D(x, y, i), texture, Tree3dmodel);
                height = i;
            });

            int width = 5;
            int maxheight = height + 3;

            Enumerable.Range(x - 2, 5).ToList().ForEach(i =>
            {
                Enumerable.Range(y - 2, 5).ToList().ForEach(j =>
                {
                    AddCube(new Point3D(i, j, height), texture2, Tree3dmodel);
                });
            });

            Enumerable.Range(x - 1, 3).ToList().ForEach(i =>
            {
                Enumerable.Range(y - 1, 3).ToList().ForEach(j =>
                {
                    AddCube(new Point3D(i, j, height + 1), texture2, Tree3dmodel);
                });
            });
        }

        public void GenerateTree(int startX, int startY, int endX, int endY, int numTrees)
        {
            int gridWidth = endX - startX;
            int gridHeight = endY - startY;

            int stepX = gridWidth / numTrees;
            int stepY = gridHeight / numTrees;

            for (int row = 0; row < numTrees; row++)
            {
                for (int column = 0; column < numTrees; column++)
                {
                    int x = startX + row * stepX;
                    int y = startY + column * stepY;
                    MakeTreemodel(x, y);
                }
            }
        }



        private void AddCube(Point3D position, ImageBrush texture, Model3DGroup parentModel)
        {
            var material = new DiffuseMaterial(texture);
            var meshBuilder = new MeshBuilder();
            meshBuilder.AddBox(new Point3D(0, 0, 0), 1, 1, 1);
            var geometry = meshBuilder.ToMesh();
            var cubeVisual = new GeometryModel3D(geometry, material);
            cubeVisual.Transform = new TranslateTransform3D(position.X, position.Y, position.Z);
            parentModel.Children.Add(cubeVisual);
        }

        public Model3DGroup Tree3dMODEL => Tree3dmodel;
    }
}
