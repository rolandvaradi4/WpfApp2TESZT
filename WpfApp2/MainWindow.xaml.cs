﻿using System;
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
using WpfApp2.Models;

using WpfApp2.Models.Textures;

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

        private Point3D currentPosition;


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            // Hides the cursor when the window loads
            Cursor = Cursors.Cross;

        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                BlocksMenu blocksMenu = new BlocksMenu(TextureID.Grass, blockClickHandler);
                blocksMenu.Show();
            }
        }


        public void HookUpEvents()
        {
            // Gets called when the window loads, will hook up events from the proper handlers to the actions.
            tickHandler.timer.Tick += Timer_Tick;
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
            cameraHandler = new CameraHandlers(this, tickHandler);
            blockClickHandler = new BlockClickHandler(viewport, cameraHandler, mapChunk);
           

            Keyboard.Focus(this);
        }


      
        

        public double StartNumCubeX = -10;
        public double StartNumCubeY = -10;
        public int NumCubeX = 20;
        public int NumCubeY = 20;

        public MapChunk mapChunk = new MapChunk(10, 10, 0, 0, Globals.PLAYER_CAMERA.LookDirection);

        private List<MapChunk> visibleMapChunks = new List<MapChunk>(); // lista a látható térképrészletekről

        private void Timer_Tick(object sender, EventArgs e)
        {

            Point3D newPosition = cameraHandler.playerCamera.Position;

            if (IsCameraAtMapEdge(newPosition, mapChunk, cameraHandler.playerCamera.LookDirection))
            {
                // Calculate the start and end positions of the new map chunk
                 StartNumCubeX = newPosition.X;
                 StartNumCubeY = newPosition.Y;

                // Generate a new map chunk at the player's position
                MapChunk newMapChunk = new MapChunk(NumCubeX, NumCubeY, (int)StartNumCubeX, (int)StartNumCubeY, cameraHandler.playerCamera.LookDirection);

                // Add the new map chunk to the children of the viewport
                viewport.Children.Add(new ModelVisual3D { Content = newMapChunk.CubeInstances });

                // Remove the non-visible map chunks from the children of the viewport
                List<MapChunk> nonVisibleChunks = visibleMapChunks.Except(new List<MapChunk> { newMapChunk }).ToList();

                viewport.Children.Cast<ModelVisual3D>()
                    .Where(modelVisual => nonVisibleChunks.Any(chunk => modelVisual.Content == chunk.CubeInstances))
                    .ToList()
                    .ForEach(modelVisual => viewport.Children.Remove(modelVisual));

                visibleMapChunks = visibleMapChunks.Except(nonVisibleChunks).ToList();

                var modelVisualToRemove = viewport.Children
                    .OfType<ModelVisual3D>()
                    .FirstOrDefault(modelVisual => modelVisual.Content == mapChunk.CubeInstances);

                if (modelVisualToRemove != null)
                {
                    viewport.Children.Remove(modelVisualToRemove);
                }


                // Set the new map chunk as the current one
                mapChunk = newMapChunk;
                visibleMapChunks.Add(mapChunk);
            }

            currentPosition = newPosition;

            // Check if CubeBlocks are in the viewport.Children collection
            bool cubeBlocksPresent = viewport.Children
      .OfType<ModelVisual3D>()
      .Any(modelVisual => modelVisual.Content == blockClickHandler.CubeBlocks);


            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                // Call the RemoveBlock method of BlockClickHandler
                blockClickHandler.RemoveBlock(viewport, cameraHandler, mapChunk);

                // Remove CubeBlocks from the viewport if they are present

                viewport.Children.Remove(new ModelVisual3D { Content = blockClickHandler.CubeBlocks });

            }
            else if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                // Call the AddBlock method of BlockClickHandler
                blockClickHandler.AddBlock(viewport, cameraHandler, mapChunk);

                // Add CubeBlocks to the viewport if they are not present
                if (!cubeBlocksPresent)
                {
                    viewport.Children.Add(new ModelVisual3D { Content = blockClickHandler.CubeBlocks });
                }
            }

            viewport.InvalidateVisual(); // update the viewport content
        }
        private bool IsCameraAtMapEdge(Point3D cameraPosition, MapChunk mapChunk , Vector3D lookDirection)
        {
            double edgeThreshold = 2.0; // Adjust this value to change the threshold distance from the map's edge
            double maxX = double.NegativeInfinity;
            double maxY = double.NegativeInfinity;
            double minX = double.PositiveInfinity;
            double minY = double.PositiveInfinity;
            foreach (var item in mapChunk.CubeInstances.Children)
            {
                if(item.Bounds.X> maxX)
                {
                    maxX = item.Bounds.X;
                }
                if (item.Bounds.Y > maxY)
                {
                    maxY = item.Bounds.Y;
                }
                if(item.Bounds.X<minX)
                {
                    minX = item.Bounds.X;
                }
                if (item.Bounds.Y < minY)
                {
                    minY = item.Bounds.Y;
                }
            }
           
            if (cameraPosition.X > maxX+2 ||cameraPosition.X < minX || cameraPosition.Y > maxY+3 || cameraPosition.Y < minY || (cameraPosition.X==0 && cameraPosition.Y==0))
            {
               
               return true;
            }

            return false;
        }
    }
}