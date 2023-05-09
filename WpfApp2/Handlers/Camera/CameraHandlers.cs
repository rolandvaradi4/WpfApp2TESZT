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
using WpfApp2.Config;

namespace WpfApp2.Handlers.Camera
{
    public  class CameraHandlers
    {
        private MainWindow mainWindow;
        private Viewport3D viewport;
        private PerspectiveCamera playerCamera;
        private Point lastMousePosition;

        public CameraHandlers(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            this.viewport = mainWindow.viewport;
            playerCamera = Globals.PLAYER_CAMERA;
        }

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        public void GameViewport_MouseMove(object sender, MouseEventArgs e)
        {
            


            Point currentMousePos = e.GetPosition(mainWindow);
            Vector mouseDelta = currentMousePos - lastMousePosition;

            // Reset the mouse position to the center of the window
             Point center = new Point(mainWindow.ActualWidth / 2, mainWindow.ActualHeight / 2);
            lastMousePosition = center;
            Point screenCenter =mainWindow.PointToScreen(center);
            SetCursorPos((int)screenCenter.X, (int)screenCenter.Y);

            Matrix3D rotationMatrix = new Matrix3D();
            // Rotate around the up vector (yaw)
            rotationMatrix.Rotate(new Quaternion(playerCamera.UpDirection, -mouseDelta.X * Globals.CAMERA_ROTATE_SPEED));
            // Rotate around the right vector (pitch)
            rotationMatrix.Rotate(new Quaternion(Vector3D.CrossProduct(playerCamera.UpDirection, playerCamera.LookDirection), mouseDelta.Y * Globals.CAMERA_ROTATE_SPEED));
            // Apply the rotation matrix to the camera's LookDirection and UpDirection vectors
            playerCamera.LookDirection = rotationMatrix.Transform(playerCamera.LookDirection);


        }
        public async Task RotateCameraAsync(double angle, Vector3D axis, Key key)
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
                    playerCamera.LookDirection = matrix.Transform(playerCamera.LookDirection);
                    playerCamera.UpDirection = matrix.Transform(playerCamera.UpDirection);
                }));

                // Wait for a short period of time to allow the camera to rotate smoothly
                await Task.Delay(20);
            }
        }
        public  async Task MoveCameraAsync(Key key)
        {
            PerspectiveCamera playerCamera = Globals.PLAYER_CAMERA;
            // Set the current speed to zero initially
            double current_speed = 0;

            // Keep moving the camera while the key is held down
            while (Keyboard.IsKeyDown(key))
            {
                // Increase the current speed up to the maximum speed
                if (current_speed < Globals.CAMERA_MOVE_SPEED)
                {
                    current_speed += Globals.CAMERA_ACCELERATION;
                }

                // Use the dispatcher to update the camera on the UI thread
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    // Move the camera in the specified direction by the current speed
                    switch (key)
                    {
                        case Key.W:
                            playerCamera.Position += playerCamera.LookDirection * current_speed;
                            break;
                        case Key.A:
                            playerCamera.Position -= Vector3D.CrossProduct(playerCamera.LookDirection, playerCamera.UpDirection) * current_speed;
                            break;
                        case Key.S:
                            playerCamera.Position -= playerCamera.LookDirection * current_speed;
                            break;
                        case Key.D:
                            playerCamera.Position += Vector3D.CrossProduct(playerCamera.LookDirection, playerCamera.UpDirection) * current_speed;
                            break;
                        case Key.Space:
                            playerCamera.Position += playerCamera.UpDirection * current_speed;
                            break;
                        case Key.LeftCtrl:
                            playerCamera.Position += -playerCamera.UpDirection * current_speed;
                            break;
                    }
                }));

                // Wait for a short period of time to allow the camera to move smoothly
                await Task.Delay(20);
            }
        }
        public async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    break;
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
                    await RotateCameraAsync(-Globals.CAMERA_ROTATE_SPEED, Vector3D.CrossProduct(playerCamera.UpDirection, playerCamera.LookDirection), Key.Up); // Rotate the camera up when the up arrow key is pressed
                    break;
                case Key.Down:
                    await RotateCameraAsync(Globals.CAMERA_ROTATE_SPEED, Vector3D.CrossProduct(playerCamera.UpDirection, playerCamera.LookDirection), Key.Down); // Rotate the camera down when the down arrow key is pressed
                    break;
                case Key.Left:
                    await RotateCameraAsync(Globals.CAMERA_ROTATE_SPEED, playerCamera.UpDirection, Key.Left); // Rotate the camera left when the left arrow key is pressed
                    break;
                case Key.Right:
                    await RotateCameraAsync(-Globals.CAMERA_ROTATE_SPEED, playerCamera.UpDirection, Key.Right); // Rotate the camera right when the right arrow key is pressed
                    break;
            }
        }
    }
}
