using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using c_sharp_text_realtime_game.Interfaces;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace c_sharp_text_realtime_game
{
    class Program
    {
        public static string path = Path.Combine(Environment.CurrentDirectory, "save.json");

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

            Character Character11 = new Zombie("test11");
            Character Character12 = new Warrior("test12");
            Character Character13 = new Robot("test13");
            Character Character14 = new Vampire("test14");
            Character Character15 = new Priest("test15");
            Character Character16 = new Assassin("test16");
            Character Character17 = new Alchimist("test17");
            Character Character18 = new Necromancer("test18");

            //Character Character11 = new Zombie("Mazimo");
            //Character Character12 = new Warrior("François");
            //Character Character13 = new Robot("Cyborki");
            //Character Character14 = new Assassin("Artham");
            //Character Character15 = new Assassin("Blake");
            //Character Character16 = new Alchimist("Ivyness");
            //Character Character17 = new Necromancer("Granmerkal");



            List<Character> characters1 = new List<Character>() {
                Character1, Character2, Character3, Character4,
                Character5, Character6, Character7, Character8
            };
            List<Character> characters2 = new List<Character>() {
                Character11, Character12, Character13, Character14,
                Character15, Character16, Character17, Character18
            };


            CancellationTokenSource Cancel1 = new CancellationTokenSource();
            CancellationTokenSource Cancel2 = new CancellationTokenSource();

            Fight fight1 = new Fight(characters1, ConsoleKey.Escape, Cancel1);
            Fight fight2 = new Fight(characters2, ConsoleKey.C, Cancel2);




            List<Character> characters3 = new List<Character>();

            if (File.Exists(path))
            {
                characters3 = LoadCharactersFromJson();
            }
            else
            {
                characters3 = new List<Character>() {
                    Character1, Character2, Character3, Character4,
                    Character5, Character6, Character7, Character8
                };
            }
            Fight fight3 = new Fight(characters3, ConsoleKey.Escape, Cancel2);

            List<Fight> fights = new List<Fight>()
            {
                fight3
            };

            FightManager fightManager = new FightManager(fights);
            await fightManager.StartMultipleFight();
        }

        static public List<Character> LoadCharactersFromJson()
        {
            List<Character> results = new List<Character>();
            string data = File.ReadAllText(path);

            List<CharacterData> characters = JsonConvert.DeserializeObject<List<CharacterData>>(data);

            // récup la classe
            foreach (CharacterData characterData in characters)
            {
                if (characterData.CurrentLife > 0)
                {
                    Type type = Type.GetType("c_sharp_text_realtime_game." + characterData.Type);
                    Character character = (Character)Activator.CreateInstance(type, characterData.Name);
                    character.AttackRate = characterData.AttackRate;
                    character.DefenseRate = characterData.DefenseRate;
                    character.AttackSpeed = characterData.AttackSpeed;
                    character.DamageRate = characterData.DamageRate;
                    character.MaximumLife = characterData.MaximumLife;
                    character.CurrentLife = characterData.CurrentLife;
                    character.PowerSpeed = characterData.PowerSpeed;
                    character.DelayAttacks = characterData.DelayAttacks;
                    character.PoisonDamages = characterData.PoisonDamages;
                    character.IsSpecialSpellAvailable = characterData.IsSpecialSpellAvailable;
                    character.Color = characterData.Color;

                    results.Add(character);
                }
            }

            return results;
        }
    }
}

