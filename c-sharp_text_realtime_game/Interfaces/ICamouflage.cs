using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace c_sharp_text_realtime_game
{
    public interface ICamouflage
    {
        public string Name { get; set; }
        public bool IsCamouflaged { get; set; }
        public Timer CamouflagedTimer { get; set; }
        public List<Character> Characters { get; set; }



        public void Camouflage()
        {
            if (IsCamouflaged == false && this.Characters.Count > 5)
            {
                Console.WriteLine("{0} se camoufle", this.Name);
                IsCamouflaged = true;
            }
            CamouflagedEventTimer();
        }
        public void CamouflagedEventTimer()
        {
            CamouflagedTimer.Elapsed += CamouflagedEvent;
            CamouflagedTimer.Interval = 5000;
            CamouflagedTimer.Enabled = true;
        }
        public void CamouflagedEvent(object source, ElapsedEventArgs e)
        {
            CamouflagedTimer.Enabled = false;
            CamouflagedTimer.Elapsed -= CamouflagedEvent;
            if (IsCamouflaged)
            {
                Console.WriteLine("{0} perd son camouflage", this.Name);
                IsCamouflaged = false;
            }
        }
    }
}
