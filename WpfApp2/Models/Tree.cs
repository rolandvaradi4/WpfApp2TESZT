using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp2.Models.Textures;
using System.Windows.Input;

namespace WpfApp2.Models
{
    internal class Tree
    {
        private readonly Model3DGroup Tree3dmodel = new Model3DGroup();

            public void MakeTreemodel(int Startx, int Starty, int endX, int endY)
            {
                BitmapImage imagea = TextureID.Tree;
                ImageBrush texture = new ImageBrush(imagea);
                BitmapImage imagea2 = TextureID.Leaf;
                ImageBrush texture2 = new ImageBrush(imagea2);
                Random random = new Random();

            int x;
            if(endX>Startx)
            {
                x= random.Next(Startx, endX);
            }
            else
            {
                x = random.Next(endX, Startx);
            }
            
            random = new Random();
            int y;
            if (endY>Starty)
            {
               y = random.Next(Starty, endY);
            }
            else
            {
                y = random.Next(endY, Starty);
            }
           


            int height = 0;
                Enumerable.Range(0, 4).ToList().ForEach(i =>
                {
                    AddCube(new Point3D(x, y, i), texture, Tree3dmodel);
                    height = i;
                });

                int width = 5;
                int maxheight = height + 3;

                Enumerable.Range(x-2, 5).ToList().ForEach(i =>
                {
                    Enumerable.Range(y-2, 5).ToList().ForEach(j =>
                    {
                        AddCube(new Point3D(i, j, height), texture2, Tree3dmodel);
                    });
                });

                Enumerable.Range(x-1, 3).ToList().ForEach(i =>
                {
                    Enumerable.Range(y-1,3).ToList().ForEach(j =>
                    {
                        AddCube(new Point3D(i, j, height + 1), texture2, Tree3dmodel);
                    });
                });

               
            }
            
        public void GenerateTree(int Startx, int Starty, int endX, int endY, int round)
        {
            for (int i = 0; i < round; i++)
            {
                MakeTreemodel(Startx, Starty, endX, endY);
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
