using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Character Character1 = new Zombie("Azouma");
            Character Character2 = new Warrior("Hector");
            Character Character3 = new Robot("Robocop");
            Character Character4 = new Vampire("Draxi");
            Character Character5 = new Priest("Simon");
            Character Character6 = new Assassin("Connor");


            Character Character11 = new Zombie("Mazimo");
            Character Character12 = new Warrior("François");
            Character Character13 = new Robot("Cyborki");
            Character Character14 = new Assassin("Artham");
            Character Character15 = new Assassin("Blake");

            //List<Character> Characters = new List<Character>() { Character1, Character2 };
            List<Character> Characters = new List<Character>() {
                Character1, Character2, Character3, Character4,
                Character5, Character6
            };
            FightManager FightManager = new FightManager(Characters);

            await FightManager.Start();
        }
    }
}

