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
        public MainWindow()
        {
            InitializeComponent();
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            // Initialize the camera
            // Access the PLAYER_CAMERA from the Globals class
            PerspectiveCamera playerCamera = Globals.PLAYER_CAMERA;
            viewport.Camera = playerCamera;
            cameraHandler = new CameraHandlers(this);    
            
            Keyboard.Focus(this);
            HookUpEvents();

        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            Point3D newPosition = cameraHandler.playerCamera.Position;

            if (Math.Floor(newPosition.X / mapChunk.chunkSize) != Math.Floor(currentPosition.X / mapChunk.chunkSize) ||
                Math.Floor(newPosition.Y / mapChunk.chunkSize) != Math.Floor(currentPosition.Y / mapChunk.chunkSize))
            {
                // Player has moved out of the current map chunk, generate a new one
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