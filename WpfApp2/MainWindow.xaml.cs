using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using WpfApp2.Config;
using WpfApp2.Handlers.Camera;
using WpfApp2.Handlers.MapGen;
using WpfApp2.Handlers.Mouse;
using WpfApp2.Handlers.TickRate;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CameraHandlers cameraHandler;
        private BlockClickHandler blockClickHandler;
        private TickHandler tickHandler;
        private Map map;

        private Point3D currentPosition;
        

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            // Hides the cursor when the window loads
            Cursor = Cursors.None;
        }

        public void HookUpEvents()
        {
            // Gets called when the window loads, will hook up events from the proper handlers to the actions.
            MouseMove += cameraHandler.GameViewport_MouseMove;

        }
        public MainWindow()
        {
            InitializeComponent();
            Initialise();
            HookUpEvents();

        }
        public void Initialise()
        {
            List<Rect3D> cubeBoundingBoxes = new List<Rect3D>();

            // Initialize the camera
            // Access the PLAYER_CAMERA from the Globals class
            PerspectiveCamera playerCamera = Globals.PLAYER_CAMERA;
            viewport.Camera = playerCamera;
            viewport.ClipToBounds = false;
            viewport.IsHitTestVisible = false;
            tickHandler = new TickHandler();
            cameraHandler = new CameraHandlers(this,tickHandler);
            Keyboard.Focus(this);
            InitialiseMap();
            ;
        }

        public void InitialiseMap()
        {
            this.map = new Map();
            ;
        }

       



        
       

    }
}