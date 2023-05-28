using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace WpfApp2.Models
{
    public class Skybox
    {
        public Model3D CreateSkybox()
        {
            // Create the skybox mesh back
            MeshGeometry3D meshGeometry3DBottom = new MeshGeometry3D();
            meshGeometry3DBottom.Positions = new Point3DCollection(new Point3D[]
           {
                new Point3D(500, 500, -500),
                new Point3D(-500, 500, -500),
                new Point3D(500, -500, -500),
                new Point3D(-500, -500, -500),
           });
            meshGeometry3DBottom.TriangleIndices = new Int32Collection(new int[]
            {
               0 , 1,  2,  2,  1,  3,
            });
            meshGeometry3DBottom.TextureCoordinates = new PointCollection(new Point[]
             {

               new Point(1, 1),
    new Point(0, 1),
    new Point(1, 0),
    new Point(0, 0),
             });

            BitmapImage bottom = new BitmapImage(new Uri("pack://application:,,,/Models/skyboxBottom.png", UriKind.RelativeOrAbsolute));
            GeometryModel3D GeometryBottom = new GeometryModel3D(meshGeometry3DBottom, new DiffuseMaterial(new ImageBrush(bottom)));


            MeshGeometry3D meshGeometry3DTop = new MeshGeometry3D();
            meshGeometry3DTop.Positions = new Point3DCollection(new Point3D[]
           {
                new Point3D(-500, 500, 500),
                new Point3D(500, 500, 500),
                new Point3D(-500, -500, 500),
                new Point3D(500, -500, 500),
           });
            meshGeometry3DTop.TriangleIndices = new Int32Collection(new int[]
            {
               0 , 1,  2,  2,  1,  3,
            });
            meshGeometry3DTop.TextureCoordinates = new PointCollection(new Point[]
             {

                new Point(0, 0),
                new Point(1, 0),
                new Point(0, 1),
                new Point(1, 1),
             });

            BitmapImage Top = new BitmapImage(new Uri("pack://application:,,,/Models/skyboxTop.png", UriKind.RelativeOrAbsolute));
            GeometryModel3D GeometryTop = new GeometryModel3D(meshGeometry3DTop, new DiffuseMaterial(new ImageBrush(Top)));


            MeshGeometry3D meshGeometry3DLeft = new MeshGeometry3D();
            meshGeometry3DLeft.Positions = new Point3DCollection(new Point3D[]
           {
                new Point3D(-500, 500, -500),
                new Point3D(-500, 500, 500),
                new Point3D(-500, -500, -500),
                new Point3D(-500, -500, 500),
           });
            meshGeometry3DLeft.TriangleIndices = new Int32Collection(new int[]
            {
               0 , 1,  2,  2,  1,  3,
            });
            meshGeometry3DLeft.TextureCoordinates = new PointCollection(new Point[]
             {

             new Point(0, 1),
    new Point(0, 0),
    new Point(1, 1),
    new Point(1, 0),
             });

            BitmapImage Left = new BitmapImage(new Uri("pack://application:,,,/Models/skyboxLeft.png", UriKind.RelativeOrAbsolute));
            GeometryModel3D GeometryLeft = new GeometryModel3D(meshGeometry3DLeft, new DiffuseMaterial(new ImageBrush(Left)));

            MeshGeometry3D meshGeometry3DRight = new MeshGeometry3D();
            meshGeometry3DRight.Positions = new Point3DCollection(new Point3D[]
           {
                new Point3D(500, 500, 500),
                new Point3D(500, 500, -500),
                new Point3D(500, -500, 500),
                new Point3D(500, -500, -500),
           });
            meshGeometry3DRight.TriangleIndices = new Int32Collection(new int[]
            {
               0 , 1,  2,  2,  1,  3,
            });
            meshGeometry3DRight.TextureCoordinates = new PointCollection(new Point[]
             {

            new Point(1, 0),
    new Point(1, 1),
    new Point(0, 0),
    new Point(0, 1),
             });

            BitmapImage Right = new BitmapImage(new Uri("pack://application:,,,/Models/skyboxRight.png", UriKind.RelativeOrAbsolute));
            GeometryModel3D GeometryRight = new GeometryModel3D(meshGeometry3DRight, new DiffuseMaterial(new ImageBrush(Right)));


            MeshGeometry3D meshGeometry3DBack = new MeshGeometry3D();
            meshGeometry3DBack.Positions = new Point3DCollection(new Point3D[]
           {
                new Point3D(-500, 500, -500),
                new Point3D(500, 500, -500),
                new Point3D(-500, 500, 500),
                new Point3D(500, 500, 500),
           });
            meshGeometry3DBack.TriangleIndices = new Int32Collection(new int[]
            {
               0 , 1,  2,  2,  1,  3,
            });
            meshGeometry3DBack.TextureCoordinates = new PointCollection(new Point[]
             {

                new Point(1, 1),
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, 0),
             });

            BitmapImage Back = new BitmapImage(new Uri("pack://application:,,,/Models/skyboxBack.png", UriKind.RelativeOrAbsolute));
            GeometryModel3D GeometryBack = new GeometryModel3D(meshGeometry3DBack, new DiffuseMaterial(new ImageBrush(Back)));


            MeshGeometry3D meshGeometry3DFront = new MeshGeometry3D();
            meshGeometry3DFront.Positions = new Point3DCollection(new Point3D[]
           {
                new Point3D(500, -500, -500),
                new Point3D(-500, -500, -500),
                new Point3D(500, -500, 500),
                new Point3D(-500, -500, 500),
           });
            meshGeometry3DFront.TriangleIndices = new Int32Collection(new int[]
            {
               0 , 1,  2,  2,  1,  3,
            });
            meshGeometry3DFront.TextureCoordinates = new PointCollection(new Point[]
             {
                new Point(1, 1),
                new Point(0, 1),
                new Point(1, 0),
                new Point(0, 0),
             });

            BitmapImage Front = new BitmapImage(new Uri("pack://application:,,,/Models/skyboxFront.png", UriKind.RelativeOrAbsolute));
            GeometryModel3D GeometryFront = new GeometryModel3D(meshGeometry3DFront, new DiffuseMaterial(new ImageBrush(Front)));

            Model3DGroup model3DGroup = new Model3DGroup();
            model3DGroup.Children.Add(GeometryBack);
            model3DGroup.Children.Add(GeometryBottom);
            model3DGroup.Children.Add(GeometryTop);
            model3DGroup.Children.Add(GeometryLeft);
            model3DGroup.Children.Add(GeometryRight);
            model3DGroup.Children.Add(GeometryFront);

            return model3DGroup;
        }
    }
}