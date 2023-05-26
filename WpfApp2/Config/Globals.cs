using System;
using System.Collections.Concurrent;
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
        private static ConcurrentDictionary<string, double> settings = new ConcurrentDictionary<string, double>();

        private static PerspectiveCamera playerCamera;

        public static PerspectiveCamera PLAYER_CAMERA
        {
            get
            {
                if (playerCamera == null)
                {
                    playerCamera = new PerspectiveCamera(new Point3D(0, 0, 3), new Vector3D(0, 1, 0), new Vector3D(0, 0, 1), Globals.FOV);
                }
                return playerCamera;
            }
        }

        static Globals()
        {
            settings.TryAdd("FOV", 90);
            settings.TryAdd("CameraMoveSpeed", 0.2);
            settings.TryAdd("CameraRotateSpeed", 0.1);
            settings.TryAdd("GravityRate", 0.05);
            settings.TryAdd("ChunkSize", 8);
            settings.TryAdd("TargetFPS", 60);
        }

        public static int FOV
        {
            get => (int)settings["FOV"];
            set => settings["FOV"] = value;
        }

        public static double CameraMoveSpeed
        {
            get => settings["CameraMoveSpeed"];
            set => settings["CameraMoveSpeed"] = value;
        }

        public static double CameraRotateSpeed
        {
            get => settings["CameraRotateSpeed"];
            set => settings["CameraRotateSpeed"] = value;
        }

        public static double GravityRate
        {
            get => settings["GravityRate"];
            set => settings["GravityRate"] = value;
        }

        public static int ChunkSize
        {
            get => (int)settings["ChunkSize"];
            set => settings["ChunkSize"] = value;
        }

        public static int TargetFPS
        {
            get => (int)settings["TargetFPS"];
            set => settings["TargetFPS"] = value;
        }
    }
}
