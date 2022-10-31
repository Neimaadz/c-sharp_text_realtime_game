using c_sharp_realtime_game.Characters;
using System;
using System.Collections.Generic;

namespace c_sharp_realtime_game
{
    class Program
    {
        static async void Main(string[] args)
        {
            Character Character1 = new Warrior("Simon");
            Character Character2 = new Warrior("Hector");

            List<Character> Characters = new List<Character>() { Character1, Character2 };
            FightManager FightManager = new FightManager(Characters);

            await FightManager.Start();
        }
    }
}

