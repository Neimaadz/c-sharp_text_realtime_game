using c_sharp_realtime_game;
using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_text_realtime_game.Characters
{
    public class Zombie : Undead
    {
        public Zombie(string name) : base(name, 150, 0, 1, 20, 1500, 1500, 0.1)
        {
        }
    }
}
