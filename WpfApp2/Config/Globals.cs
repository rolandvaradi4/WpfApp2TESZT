using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace WpfApp2.Config
{
    public static class Globals
    {
        private static int FOV = 90;
        public static PerspectiveCamera PLAYER_CAMERA= new PerspectiveCamera(new Point3D(0, 0, 3), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1), FOV);
        public static double CAMERA_MOVE_SPEED = 0.2;
        public static double CAMERA_ROTATE_SPEED = 0.1;
        public static double GRAVITY_RATE = 0.1;


        public static int TargetFPS = 60; // pl. 30 FPS

    }
}
