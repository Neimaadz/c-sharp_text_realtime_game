using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_text_realtime_game
{
    public class RemainingCharactersEventArgs : EventArgs
    {
        public List<Character> RemainingCharacters;
        public RemainingCharactersEventArgs()
        {
        }
    }
}
