using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Define camera movement speed
        private const double CameraMoveSpeed = 0.05;
        private const double CameraRotateSpeed = 0.2;
        private const double CameraAcceleration = 0.001;

        // Define camera properties
        private PerspectiveCamera camera;
        private double moveSpeed = 0.1;
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
                    await MoveCameraAsync(Key.W);
                    break;
                case Key.A:
                    await MoveCameraAsync(Key.A);
                    break;
                case Key.S:
                    await MoveCameraAsync(Key.S);
                    break;
                case Key.D:
                    await MoveCameraAsync(Key.D);
                    break;
            }
        }

        private async Task MoveCameraAsync(Key key)
        {
            double current_speed = 0;
            while (Keyboard.IsKeyDown(key))
            {
                if (current_speed < CameraMoveSpeed)
                {
                    current_speed += CameraAcceleration;
                }

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
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
                    }
                }));

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
                Vector3D cameraMove = new Vector3D(mouseDelta.X * CameraMoveSpeed, 0, -mouseDelta.Y * CameraMoveSpeed);
                camera.Position += camera.Transform.Transform(cameraMove);

                lastMousePosition = currentMousePos;
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
