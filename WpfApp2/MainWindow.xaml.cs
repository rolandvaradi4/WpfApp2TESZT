using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using WpfApp2.Handlers.MapGen;
using WpfApp2.Models.Textures;
using WpfApp2.Config;
using WpfApp2.Handlers.Camera;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        // Define camera properties
        private Point lastMousePosition;
        List<Rect3D> cubeBoundingBoxes = new List<Rect3D>();
        public MainWindow()
        {
            InitializeComponent();
            

            foreach (ModelVisual3D test in viewport.Children)
            {
                Rect3D boundingBox = test.Content.Bounds;
                cubeBoundingBoxes.Add(boundingBox);
            }
            BitmapImage textureImage = TextureID.Grass;
            ImageBrush image = new ImageBrush(textureImage);
            var myMap = new map(50, 50, image);
            var modelVisual = new ModelVisual3D();
            modelVisual.Content = myMap.CubeInstances;
            viewport.Children.Add(modelVisual);
            // Initialize the camera
            // Access the PLAYER_CAMERA from the Globals class
            PerspectiveCamera playerCamera = Globals.PLAYER_CAMERA;
            viewport.Camera = playerCamera;
            CameraHandlers cameraHandler = new CameraHandlers(this);



            // Attach the KeyDown event handler
            KeyDown += cameraHandler.MainWindow_KeyDown;

            // Hook up event handlers for mouse and keyboard input
            this.MouseMove += cameraHandler.GameViewport_MouseMove;
            Keyboard.Focus(this);
            KeyDown += cameraHandler.MainWindow_KeyDown;

        }


        
        



    










        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

    }
}