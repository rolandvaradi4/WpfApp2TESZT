using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using WpfApp2.Handlers.Camera;
using WpfApp2.Models.Textures;

namespace WpfApp2.Handlers.MapGen
{
    public class MapChunk
    {
        private  Model3DGroup _cubeInstances = new Model3DGroup();
        private readonly int _numCubesX;
        private readonly int _numCubesY;
     
        private readonly map _map;
        public readonly int _startnumCubesX;
        public readonly int _starnumCubesY;

        public MapChunk(int numCubesX, int numCubesY,int startnumCubesX, int starnumCubesY, Vector3D lookDirection)
        {
            _numCubesX = numCubesX;
            _numCubesY = numCubesY;
         
            _startnumCubesX = startnumCubesX;
            _starnumCubesY = starnumCubesY;
            _map = new map(_numCubesX, _numCubesY, _startnumCubesX, _starnumCubesY, lookDirection);
            AddCubeInstances();
        }

        private void AddCubeInstances()
        {
            foreach (var child in _map.CubeInstances.Children)
            {
                _cubeInstances.Children.Add(child);
            }

        }


        public Model3DGroup CubeInstances => _cubeInstances;
    }
}
