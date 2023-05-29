using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.IO;

namespace WpfApp2.Models.Sound
{
    public static class SoundID
    {
        private static string soundFolderPath = "Models/Sound/";

        public static SoundPlayer Click = new SoundPlayer(Path.Combine(soundFolderPath, "ButtonSound.wav"));
        public static SoundPlayer Remove = new SoundPlayer(Path.Combine(soundFolderPath, "RemoveSound.wav"));
        public static SoundPlayer Add = new SoundPlayer(Path.Combine(soundFolderPath, "AddSound.wav"));
        public static SoundPlayer Walk = new SoundPlayer(Path.Combine(soundFolderPath, "Walking2.wav"));
    }
}

