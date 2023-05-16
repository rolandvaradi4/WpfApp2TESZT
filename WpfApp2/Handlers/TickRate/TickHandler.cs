using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Config;

namespace WpfApp2.Handlers.TickRate
{
    public class TickHandler
    {
       
        public readonly System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
        public TickHandler()
        {
            timer.Interval = TimeSpan.FromMilliseconds(1000.0 / Globals.TargetFPS);
            timer.Start();

        }

        

    }
}
