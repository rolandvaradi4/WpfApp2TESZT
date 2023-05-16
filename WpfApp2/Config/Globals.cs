﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WpfApp2.Config
{
    public static class Globals
    {
        public static PerspectiveCamera PLAYER_CAMERA= new PerspectiveCamera(new Point3D(0, 0, 1), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1), 60);
        public static double CAMERA_MOVE_SPEED = 0.005;
        public static double CAMERA_ROTATE_SPEED = 0.1;
        public static double CAMERA_ACCELERATION = 0.001;
        public static int TargetFPS = 30; // pl. 30 FPS

    }
}
