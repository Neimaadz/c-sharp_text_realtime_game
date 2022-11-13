using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class FightManager
    {
        public List<Character> Characters;

        public FightManager(List<Character> characters)
        {
            this.Characters = characters;
            foreach (Character character in Characters)
            {
                character.SetFightManager(this);
            }
        }


        public async Task StartBattleRoyal()
        {
            Console.WriteLine("Starting Battle Royal");

            List<Task<Character>> attackTasks = new List<Task<Character>>();
            foreach (Character character in Characters)
            {
                attackTasks.Add(character.Start());
            }

            while (attackTasks.Count > 1)
            {
                Task<Character> CharaterDead =  await Task.WhenAny(attackTasks);
                attackTasks.Remove(CharaterDead);
            }

            foreach (Character character in Characters)
            {
                Console.WriteLine("Winner is : {0}", character.Name);
                Console.WriteLine("{0} PV restant : {1} PV", character.Name, character.CurrentLife);
            }

            Console.WriteLine("Finish");

        }
        public async Task StartMultipleFight()
        {
            Console.WriteLine("Starting Multiple Fight");

            Console.WriteLine("Finish");

        }


    }
}

