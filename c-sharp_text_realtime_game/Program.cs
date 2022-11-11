using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Character Character1 = new Warrior("Simon");
            Character Character2 = new Warrior("Hector");
            Character Character3 = new Warrior("François");
            Character Character4 = new Priest("Xavier");
            Character Character5 = new Zombie("Azouma");
            Character Character6 = new Zombie("Mazimo");
            Character Character7 = new Robot("Robocop");
            Character Character8 = new Vampire("Traxi");

            //List<Character> Characters = new List<Character>() { Character1, Character2 };
            //List<Character> Characters = new List<Character>() { Character1, Character2, Character3, Character4 };
            List<Character> Characters = new List<Character>() {
                Character1, Character2, Character3, Character4,
                Character5, Character6, Character7, Character8
            };
            FightManager FightManager = new FightManager(Characters);

            await FightManager.Start();
        }
    }
}

