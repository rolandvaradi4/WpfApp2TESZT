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

        private CameraHandlers cameraHandler;
      
        private readonly MapChunk mapChunk = new MapChunk();
        private Point3D currentPosition;
        private const int TargetFPS = 30; // pl. 30 FPS
        private readonly System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            
         
            

            // Initialize the camera
            // Access the PLAYER_CAMERA from the Globals class
            PerspectiveCamera playerCamera = Globals.PLAYER_CAMERA;
            viewport.Camera = playerCamera;
            viewport.ClipToBounds = false;
            viewport.IsHitTestVisible = false;
            cameraHandler = new CameraHandlers(this);
            timer.Interval = TimeSpan.FromMilliseconds(1000.0 / TargetFPS);
            timer.Tick += Timer_Tick;
            timer.Start();

            Keyboard.Focus(this);
            HookUpEvents();

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Point3D newPosition = cameraHandler.playerCamera.Position;

            if (IsCameraAtMapEdge(newPosition))
            {
                // Player is at the edge of the map, generate a new one
                List<ModelVisual3D> newModels = mapChunk.GenerateChunk(newPosition);
                foreach (var model in newModels)
                {
                    // Check if the model is already in the children collection of the viewport
                    if (!viewport.Children.Contains(model))
                    {
                        viewport.Children.Add(model);
                    }
                }

                // Update the chunk by removing the old models and generating new ones
                List<ModelVisual3D> updatedModels = mapChunk.UpdateChunk(newPosition, viewport);
                foreach (var model in updatedModels)
                {
                    if (!viewport.Children.Contains(model))
                    {
                        viewport.Children.Add(model);
                    }
                }

            }

            currentPosition = newPosition;

            viewport.InvalidateVisual(); // frissítjük a viewport tartalmát
        }





        private bool IsCameraAtMapEdge(Point3D cameraPosition)
        {
            double halfMapSize= mapChunk.chunkSize / 2;
            double x = cameraPosition.X;
            double y = cameraPosition.Y;
          
        
            if (x < -halfMapSize || x > halfMapSize || y < -halfMapSize || y > halfMapSize || (x==0 && y==0))
            {
                return true;
            }

            return false;
        }



        public void HookUpEvents()
        {
            KeyDown += cameraHandler.MainWindow_KeyDown;
            MouseMove += cameraHandler.GameViewport_MouseMove;
            KeyDown += cameraHandler.MainWindow_KeyDown;
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

    }
}