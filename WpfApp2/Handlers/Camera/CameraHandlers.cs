using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
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
using WpfApp2.Handlers.MapGen;
using WpfApp2.Models.Textures;

namespace WpfApp2.Handlers.Camera
{
    public class CameraHandlers
    {
        private MainWindow mainWindow;
        private Viewport3D viewport;
        public PerspectiveCamera playerCamera;
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
            System.Windows.Vector mouseDelta = currentMousePos - lastMousePosition;

            // Reset the mouse position to the center of the window
            Point center = new Point(mainWindow.ActualWidth / 2, mainWindow.ActualHeight / 2);
            lastMousePosition = center;
            Point screenCenter = mainWindow.PointToScreen(center);
            SetCursorPos((int)screenCenter.X, (int)screenCenter.Y);

            Matrix3D rotationMatrix = new Matrix3D();
            // Rotate around the up vector (yaw)
            rotationMatrix.Rotate(new System.Windows.Media.Media3D.Quaternion(playerCamera.UpDirection, -mouseDelta.X * Globals.CAMERA_ROTATE_SPEED));
            // Rotate around the right vector (pitch)
            rotationMatrix.Rotate(new System.Windows.Media.Media3D.Quaternion(Vector3D.CrossProduct(playerCamera.UpDirection, playerCamera.LookDirection), mouseDelta.Y * Globals.CAMERA_ROTATE_SPEED));
            // Apply the rotation matrix to the camera's LookDirection and UpDirection vectors
            playerCamera.LookDirection = rotationMatrix.Transform(playerCamera.LookDirection);
            mainWindow.MousePositionTextBlock.Text = playerCamera.GetInfo().ToString();


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
                    matrix.Rotate(new System.Windows.Media.Media3D.Quaternion(axis, angle));
                    // Use the matrix to rotate the camera's look direction and up direction
                    playerCamera.LookDirection = matrix.Transform(playerCamera.LookDirection);
                    playerCamera.UpDirection = matrix.Transform(playerCamera.UpDirection);
                }));

                // Wait for a short period of time to allow the camera to rotate smoothly
                await Task.Delay(20);
            }
        }
        public static Vector3D ProjectToXZPlane(Vector3D vector)
        {
            return new Vector3D(vector.X, 0, vector.Z);
        }
        public async Task MoveCameraAsync(Key key)
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
                    mainWindow.MousePositionTextBlock.Text = playerCamera.GetInfo().ToString();
                    // Move the camera in the specified direction by the current speed
                    switch (key)
                    {
                        case Key.Escape:

                            mainWindow.Close();
                            break;
                        case Key.W:
                            if (CollisionDetection())
                            {
                                // Break out of the entire case statement
                                playerCamera.Position -= new Vector3D(playerCamera.LookDirection.X, playerCamera.LookDirection.Y, 0) * Globals.CAMERA_MOVE_SPEED;
                                break;
                            }
                            current_speed = Globals.CAMERA_MOVE_SPEED; // Set the speed to the maximum
                            playerCamera.Position += new Vector3D(playerCamera.LookDirection.X, playerCamera.LookDirection.Y, 0) * current_speed;
                            break;
                        case Key.A:
                            if (CollisionDetection())
                            {
                                // Break out of the entire case statement
                                playerCamera.Position += Vector3D.CrossProduct(playerCamera.LookDirection, playerCamera.UpDirection) * current_speed;
                                playerCamera.Position += new Vector3D(0, 0, playerCamera.LookDirection.Z) * current_speed;
                                break;
                            }
                            playerCamera.Position -= Vector3D.CrossProduct(playerCamera.LookDirection, playerCamera.UpDirection) * current_speed;
                            playerCamera.Position -= new Vector3D(0, 0, playerCamera.LookDirection.Z) * current_speed;
                            break;
                        case Key.S:
                            if (CollisionDetection())
                            {
                                // Break out of the entire case statement
                                playerCamera.Position += new Vector3D(playerCamera.LookDirection.X, playerCamera.LookDirection.Y, 0) * current_speed;
                                break;
                            }
                            playerCamera.Position -= new Vector3D(playerCamera.LookDirection.X, playerCamera.LookDirection.Y, 0) * current_speed;
                            break;
                        case Key.D:
                            if (CollisionDetection())
                            {
                                // Break out of the entire case statement
                                playerCamera.Position -= Vector3D.CrossProduct(playerCamera.LookDirection, playerCamera.UpDirection) * current_speed;
                                playerCamera.Position -= new Vector3D(0, 0, playerCamera.LookDirection.Z) * current_speed;
                                break;
                            }
                            playerCamera.Position += Vector3D.CrossProduct(playerCamera.LookDirection, playerCamera.UpDirection) * current_speed;
                            playerCamera.Position += new Vector3D(0, 0, playerCamera.LookDirection.Z) * current_speed;
                            break;
                        case Key.Space:
                            if (CollisionDetection())
                            {
                                // Break out of the entire case statement
                                playerCamera.Position += playerCamera.UpDirection * current_speed;
                                break;
                            }
                            playerCamera.Position += playerCamera.UpDirection * current_speed;
                            break;
                        case Key.LeftCtrl:
                            if (CollisionDetection())
                            {
                                // Break out of the entire case statement
                                playerCamera.Position -= -playerCamera.UpDirection * current_speed;
                                break;
                            }
                            playerCamera.Position += -playerCamera.UpDirection * current_speed;
                            break;
                    }
                }));

                // Wait for a short period of time to allow the camera to move smoothly
                await Task.Delay(20);
            }
        }

        public bool CollisionDetection()
        {
            return mainWindow.viewport.Children
                .OfType<ModelVisual3D>()
                .SelectMany(chunks => ((Model3DGroup)chunks.Content).Children.OfType<GeometryModel3D>())
                .Select(geometryModel =>
                {
                    Transform3D modelTransform = geometryModel.Transform;
                    Rect3D modelBounds = GetModelBoundsRecursive(geometryModel, modelTransform);
                    Rect3D cameraBounds = new Rect3D(playerCamera.Position, new Size3D(1, 1, 1));
                    return cameraBounds.IntersectsWith(modelBounds);
                })
                .Any(hasCollision => hasCollision);
        }

        private Rect3D GetModelBoundsRecursive(GeometryModel3D model, Transform3D transform)
        {
            Rect3D bounds = Rect3D.Empty;

            // Transform the geometry bounds
            Geometry3D geometry = model.Geometry;
            Rect3D geometryBounds = geometry.Bounds;
            Point3D[] corners = GetBoundingBoxCorners(geometryBounds);

            Matrix3D matrix = transform.Value;

            // Transform all corners at once using matrix multiplication
            matrix.Transform(corners);

            // Compute the transformed bounding box
            bounds = ComputeBoundingBox(corners);

            return bounds;
        }

        private Rect3D GetModelBoundsRecursive(ModelVisual3D modelVisual, Transform3D transform)
        {
            Matrix3D matrix = transform.Value;
            var bounds = Rect3D.Empty;

            // Get bounds of all GeometryModel3D objects
            var geometryBounds = modelVisual.Children
                .OfType<ModelVisual3D>()
                .SelectMany(childVisual => childVisual.Children.OfType<GeometryModel3D>())
                .Select(geometryModel =>
                {
                    var geometry = geometryModel.Geometry;
                    var geometryBounds = geometry.Bounds;
                    var corners = GetBoundingBoxCorners(geometryBounds)
                        .Select(matrix.Transform)
                        .ToArray();
                    return ComputeBoundingBox(corners);
                });

            // Compute the union of all bounds
            foreach (var geometryBound in geometryBounds)
            {
                bounds = UnionRect3D(bounds, geometryBound);
            }

            // Recursively compute bounds of all child ModelVisual3D objects
            foreach (var childVisual in modelVisual.Children.OfType<ModelVisual3D>())
            {
                var childTransform = childVisual.Transform;
                var childBounds = GetModelBoundsRecursive(childVisual, childTransform);
                bounds = UnionRect3D(bounds, childBounds);
            }

            return bounds;
        }

        private Rect3D UnionRect3D(Rect3D rect1, Rect3D rect2)
        {
            if (rect1.IsEmpty)
                return rect2;
            if (rect2.IsEmpty)
                return rect1;

            double minX = Math.Min(rect1.X, rect2.X);
            double minY = Math.Min(rect1.Y, rect2.Y);
            double minZ = Math.Min(rect1.Z, rect2.Z);
            double maxX = Math.Max(rect1.X + rect1.SizeX, rect2.X + rect2.SizeX);
            double maxY = Math.Max(rect1.Y + rect1.SizeY, rect2.Y + rect2.SizeY);
            double maxZ = Math.Max(rect1.Z + rect1.SizeZ, rect2.Z + rect2.SizeZ);

            return new Rect3D(minX, minY, minZ, maxX - minX, maxY - minY, maxZ - minZ);
        }
        private Rect3D ComputeBoundingBox(Point3D[] corners)
        {
            double minX = double.PositiveInfinity;
            double minY = double.PositiveInfinity;
            double minZ = double.PositiveInfinity;
            double maxX = double.NegativeInfinity;
            double maxY = double.NegativeInfinity;
            double maxZ = double.NegativeInfinity;

            for (int i = 0; i < corners.Length; i++)
            {
                Point3D corner = corners[i];
                minX = Math.Min(minX, corner.X);
                minY = Math.Min(minY, corner.Y);
                minZ = Math.Min(minZ, corner.Z);
                maxX = Math.Max(maxX, corner.X);
                maxY = Math.Max(maxY, corner.Y);
                maxZ = Math.Max(maxZ, corner.Z);
            }

            return new Rect3D(minX, minY, minZ, maxX - minX, maxY - minY, maxZ - minZ);
        }

        private Point3D[] GetBoundingBoxCorners(Rect3D rect)
        {
            Point3D[] corners = new Point3D[8];
            corners[0] = rect.Location;
            corners[1] = new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z);
            corners[2] = new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z);
            corners[3] = new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z);
            corners[4] = new Point3D(rect.X, rect.Y, rect.Z + rect.SizeZ);
            corners[5] = new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z + rect.SizeZ);
            corners[6] = new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z + rect.SizeZ);
            corners[7] = new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z + rect.SizeZ);
            return corners;
        }

        public async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    this.mainWindow.Close();
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