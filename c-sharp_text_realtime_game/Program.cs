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
            Character Character7 = new Alchimist("Singed");
            Character Character8 = new Necromancer("Gromoun");


            Character Character11 = new Zombie("Mazimo");
            Character Character12 = new Warrior("François");
            Character Character13 = new Robot("Cyborki");
            Character Character14 = new Assassin("Artham");
            Character Character15 = new Assassin("Blake");
            Character Character16 = new Alchimist("Ivyness");
            Character Character17 = new Necromancer("Granmerkal");

            //List<Character> Characters = new List<Character>() { Character2, Character12 };
            /*List<Character> Characters = new List<Character>() {
                Character1, Character2, Character3, Character4,
                Character5, Character6, Character7, Character8
            };*/
            List<Character> Characters = new List<Character>() {
                Character1, Character2, Character3, Character4,
                Character5, Character6, Character7, Character8,
                Character11, Character12, Character13, Character14,
                Character15, Character16, Character17
            };
            FightManager FightManager = new FightManager(Characters);

            await FightManager.Start();
        }
    }
}

