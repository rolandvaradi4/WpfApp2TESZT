using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WpfApp2.Models.Sound
{
    public static class SoundID
    {
        private static string soundFolderPath = "Models/Sound/";

        public static MediaPlayer Click = CreateMediaPlayer("ButtonSound.wav");
        public static MediaPlayer Remove = CreateMediaPlayer("RemoveSound.wav");
        public static MediaPlayer Add = CreateMediaPlayer("AddSound.wav");
        public static MediaPlayer Walk = CreateMediaPlayer("Walking2.wav");

        private static MediaPlayer CreateMediaPlayer(string fileName)
        {
            string fullPath = System.IO.Path.Combine(soundFolderPath, fileName);
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(fullPath, UriKind.Relative));
            return mediaPlayer;
        }
    }
}
