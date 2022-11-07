using c_sharp_realtime_game.Characters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_realtime_game
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Character Character1 = new Warrior("Simon");
            Character Character2 = new Warrior("Hector");
            //Character Character3 = new Warrior("François");
            //Character Character4 = new Warrior("Xavier");

            List<Character> Characters = new List<Character>() { Character1, Character2 };
            FightManager FightManager = new FightManager(Characters);

            await FightManager.Start();
        }
    }
}

