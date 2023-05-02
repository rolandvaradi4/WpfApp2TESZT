using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private const double CameraMoveSpeed = 0.1;
        private const double CameraRotateSpeed = 0.2;

        // Define camera properties
        private PerspectiveCamera gameCamera;
        private Point lastMousePosition;

        public MainWindow()
        {
            InitializeComponent();

            // Create and set up the camera
            gameCamera = new PerspectiveCamera(new Point3D(0, 0, 5), new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 45);
            GameViewport.Camera = gameCamera;

            // Hook up event handlers for mouse and keyboard input
            GameViewport.MouseMove += GameViewport_MouseMove;
            GameViewport.MouseLeftButtonDown += GameViewport_MouseLeftButtonDown;
            GameViewport.MouseLeftButtonUp += GameViewport_MouseLeftButtonUp;
            Keyboard.Focus(this);
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    gameCamera.Position += gameCamera.LookDirection * CameraMoveSpeed;
                    break;
                case Key.A:
                    gameCamera.Position -= Vector3D.CrossProduct(gameCamera.LookDirection, gameCamera.UpDirection) * CameraMoveSpeed;
                    break;
                case Key.S:
                    gameCamera.Position -= gameCamera.LookDirection * CameraMoveSpeed;
                    break;
                case Key.D:
                    gameCamera.Position += Vector3D.CrossProduct(gameCamera.LookDirection, gameCamera.UpDirection) * CameraMoveSpeed;
                    break;
            }
        }

        private void GameViewport_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentMousePos = e.GetPosition(GameViewport);
                Vector mouseDelta = currentMousePos - lastMousePosition;

                // Adjust camera position based on mouse delta
                Vector3D cameraMove = new Vector3D(mouseDelta.X * CameraMoveSpeed, 0, -mouseDelta.Y * CameraMoveSpeed);
                gameCamera.Position += gameCamera.Transform.Transform(cameraMove);

                lastMousePosition = currentMousePos;
            }
        }

        private void GameViewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Capture the mouse to handle dragging
            GameViewport.CaptureMouse();

            // Save the current mouse position
            lastMousePosition = e.GetPosition(GameViewport);
        }

        private void GameViewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Release the mouse capture
            GameViewport.ReleaseMouseCapture();
        }
    }
}
