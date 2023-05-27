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


        private void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Call the RemoveBlock method of BlockClickHandler

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Call the RemoveBlock method of BlockClickHandler
                blockClickHandler.RemoveBlock(viewport, cameraHandler, mapChunk);
            }

        }

        private void Viewport_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Call the AddBlock method of BlockClickHandler

            if (e.RightButton == MouseButtonState.Pressed)
            {
                // Call the RemoveBlock method of BlockClickHandler
                blockClickHandler.AddBlock(viewport, cameraHandler, mapChunk);
            }
        }

        public int StartNumCubeX = 0;
        public int StartNumCubeY = 0;
        public int NumCubeX = 16;
        public int NumCubeY = 16;

        public MapChunk mapChunk = new MapChunk(10, 10, 0, 0, Globals.PLAYER_CAMERA.LookDirection);

        private List<MapChunk> visibleMapChunks = new List<MapChunk>(); // lista a látható térképrészletekről

        private void Timer_Tick(object sender, EventArgs e)
        {

            Point3D newPosition = cameraHandler.playerCamera.Position;

            if (IsCameraAtMapEdge(newPosition))
            {
                // Calculate the start and end positions of the new map chunk
                double startNumCubesX = newPosition.X;
                double startNumCubesY = newPosition.Y;

                // Generate a new map chunk at the player's position
                MapChunk newMapChunk = new MapChunk(NumCubeX, NumCubeY, (int)startNumCubesX, (int)startNumCubesY, cameraHandler.playerCamera.LookDirection);

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
        private bool IsCameraAtMapEdge(Point3D cameraPosition)
        {
            double mapSize = Math.Max(NumCubeX, NumCubeY);
            double halfMapSize = mapSize / 2.0;

            double minX = mapChunk._startnumCubesX - 3;
            double maxX = mapChunk._startnumCubesX + 10;
            double minY = mapChunk._starnumCubesY - 3;
            double maxY = mapChunk._starnumCubesY + 10;

            bool atMapEdge = cameraPosition.X < minX ||
                             cameraPosition.X > maxX ||
                             cameraPosition.Y < minY ||
                             cameraPosition.Y > maxY;

            return atMapEdge || (cameraPosition.X == 0 && cameraPosition.Y == 0);
        }

    }
}