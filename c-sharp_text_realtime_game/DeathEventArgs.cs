using System;
using c_sharp_realtime_game;

namespace c_sharp_text_realtime_game
{
    public class DeathEventArgs : EventArgs
    {
        public Character DeadCharacter;

        public DeathEventArgs()
        {
        }
    }
}

