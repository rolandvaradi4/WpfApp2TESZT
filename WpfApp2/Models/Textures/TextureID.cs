using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WpfApp2.Models.Textures
{
    public static class TextureID
    {

        public static BitmapImage Grass = new BitmapImage(new Uri("pack://application:,,,/Models/Textures/grass.png",UriKind.RelativeOrAbsolute));
        public static BitmapImage Tree = new BitmapImage(new Uri("pack://application:,,,/Models/Textures/tree.jpg", UriKind.RelativeOrAbsolute));
        public static BitmapImage Stone = new BitmapImage(new Uri("pack://application:,,,/Models/Textures/stone.png", UriKind.RelativeOrAbsolute));
        public static BitmapImage Leaf = new BitmapImage(new Uri("pack://application:,,,/Models/Textures/leaf.png", UriKind.RelativeOrAbsolute));


    }
}
