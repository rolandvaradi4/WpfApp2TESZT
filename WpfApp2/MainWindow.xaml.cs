﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define camera movement speed
        private const double CAMERA_MOVE_SPEED = 0.005;
        private const double CAMERA_ROTATE_SPEED = 0.1;
        private const double CAMERA_ACCELERATION = 0.0001;

        // Define camera properties
        private PerspectiveCamera camera;
        private Point lastMousePosition;

        public MainWindow()
        {
            InitializeComponent();


            // Initialize the camera
            camera = new PerspectiveCamera(new Point3D(0, 0, 5), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 45);
            viewport.Camera = camera;

            // Attach the KeyDown event handler
            KeyDown += MainWindow_KeyDown;

            // Hook up event handlers for mouse and keyboard input
            viewport.MouseMove += GameViewport_MouseMove;
            viewport.MouseLeftButtonDown += GameViewport_MouseLeftButtonDown;
            viewport.MouseLeftButtonUp += GameViewport_MouseLeftButtonUp;
            Keyboard.Focus(this);
            KeyDown += MainWindow_KeyDown;

        }

        private async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    await MoveCameraAsync(Key.W); // Move the camera forward when W is pressed
                    break;
                case Key.A:
                    await MoveCameraAsync(Key.A); // Move the camera left when A is pressed
                    break;
                case Key.S:
                    await MoveCameraAsync(Key.S); // Move the camera backward when S is pressed
                    break;
                case Key.D:
                    await MoveCameraAsync(Key.D); // Move the camera right when D is pressed
                    break;
                case Key.Space:
                    await MoveCameraAsync(Key.Space); // Move the camera up when Space is pressed
                    break;
                case Key.LeftCtrl:
                    await MoveCameraAsync(Key.LeftCtrl); // Move the camera down when Left Control is pressed
                    break;
                case Key.Up:
                    await RotateCameraAsync(-CAMERA_ROTATE_SPEED, Vector3D.CrossProduct(camera.UpDirection, camera.LookDirection), Key.Up); // Rotate the camera up when the up arrow key is pressed
                    break;
                case Key.Down:
                    await RotateCameraAsync(CAMERA_ROTATE_SPEED, Vector3D.CrossProduct(camera.UpDirection, camera.LookDirection), Key.Down); // Rotate the camera down when the down arrow key is pressed
                    break;
                case Key.Left:
                    await RotateCameraAsync(CAMERA_ROTATE_SPEED, camera.UpDirection, Key.Left); // Rotate the camera left when the left arrow key is pressed
                    break;
                case Key.Right:
                    await RotateCameraAsync(-CAMERA_ROTATE_SPEED, camera.UpDirection, Key.Right); // Rotate the camera right when the right arrow key is pressed
                    break;
            }
        }
        private async Task RotateCameraAsync(double angle, Vector3D axis, Key key)
        {
            // Keep rotating the camera while the key is held down
            while (Keyboard.IsKeyDown(key))
            {
                // Use the dispatcher to update the camera on the UI thread
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Create a new 3D matrix to represent the rotation
                    Matrix3D matrix = new Matrix3D();

                    // Rotate the matrix by the specified angle around the specified axis
                    matrix.Rotate(new Quaternion(axis, angle));

                    // Use the matrix to rotate the camera's look direction and up direction
                    camera.LookDirection = matrix.Transform(camera.LookDirection);
                    camera.UpDirection = matrix.Transform(camera.UpDirection);

                }));

                // Wait for a short period of time to allow the camera to rotate smoothly
                await Task.Delay(20);
            }
        }



        private async Task MoveCameraAsync(Key key)
        {
            // Set the current speed to zero initially
            double current_speed = 0;

            // Keep moving the camera while the key is held down
            while (Keyboard.IsKeyDown(key))
            {
                // Increase the current speed up to the maximum speed
                if (current_speed < CAMERA_MOVE_SPEED)
                {
                    current_speed += CameraAcceleration;
                }

                // Use the dispatcher to update the camera on the UI thread
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Move the camera in the specified direction by the current speed
                    switch (key)
                    {
                        case Key.W:
                            camera.Position += camera.LookDirection * current_speed;
                            break;
                        case Key.A:
                            camera.Position -= Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection) * current_speed;
                            break;
                        case Key.S:
                            camera.Position -= camera.LookDirection * current_speed;
                            break;
                        case Key.D:
                            camera.Position += Vector3D.CrossProduct(camera.LookDirection, camera.UpDirection) * current_speed;
                            break;
                        case Key.Space:
                            camera.Position += camera.UpDirection * current_speed;
                            break;
                        case Key.LeftCtrl:
                            camera.Position += -camera.UpDirection * current_speed;
                            break;
                    }
                }));

                // Wait for a short period of time to allow the camera to move smoothly
                await Task.Delay(20);
            }
        }


        private void GameViewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentMousePos = e.GetPosition(viewport);
                Vector mouseDelta = currentMousePos - lastMousePosition;

                // Adjust camera position based on mouse delta
                Vector3D cameraMove = new Vector3D(mouseDelta.X * CAMERA_MOVE_SPEED, 0, -mouseDelta.Y * CAMERA_MOVE_SPEED);
                camera.Position += camera.Transform.Transform(cameraMove);

                lastMousePosition = currentMousePos;
                Matrix3D rotationMatrix = new Matrix3D();

                // Rotate around the up vector (yaw)
                rotationMatrix.Rotate(new Quaternion(camera.UpDirection, -mouseDelta.X * CAMERA_ROTATE_SPEED));

                // Rotate around the right vector (pitch)
                rotationMatrix.Rotate(new Quaternion(Vector3D.CrossProduct(camera.UpDirection, camera.LookDirection), -mouseDelta.Y * CAMERA_ROTATE_SPEED));

                // Apply the rotation matrix to the camera's LookDirection and UpDirection vectors
                camera.LookDirection = rotationMatrix.Transform(camera.LookDirection);
                camera.UpDirection = rotationMatrix.Transform(camera.UpDirection);

            }
        }

        private void GameViewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse to handle dragging
            viewport.CaptureMouse();

            // Save the current mouse position
            lastMousePosition = e.GetPosition(viewport);
        }

        private void GameViewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Release the mouse capture
            viewport.ReleaseMouseCapture();
        }
    }

}