using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApp2.Models.Textures
{
    public static class TextureID
    {
        private static ConcurrentDictionary<string, BitmapImage> textures = new ConcurrentDictionary<string, BitmapImage>()
        {
            ["Grass"] = new BitmapImage(new Uri("pack://application:,,,/Models/Textures/grass.png", UriKind.RelativeOrAbsolute))
        };

        public static BitmapImage Grass
        {
            get
            {
                return textures.GetOrAdd("Grass", (_) => new BitmapImage(new Uri("pack://application:,,,/Models/Textures/grass.png", UriKind.RelativeOrAbsolute)));
            }
        }
    }
}
