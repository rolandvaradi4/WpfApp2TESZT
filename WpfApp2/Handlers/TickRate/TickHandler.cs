using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Handlers.TickRate
{
    internal class TickHandler
    {
        private const int TargetFPS = 30; // pl. 30 FPS
        public readonly System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public TickHandler()
        {
            timer.Interval = TimeSpan.FromMilliseconds(1000.0 / TargetFPS);
            timer.Start();

        }

        

    }
}
