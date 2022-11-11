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


        public async Task Start()
        {
            Console.WriteLine("Start");

            List<Task<Character>> AttackTasks = new List<Task<Character>>();
            foreach (Character character in Characters)
            {
                AttackTasks.Add(character.Start());
            }

            while (AttackTasks.Count > 1)
            {
                Task<Character> CharaterDead =  await Task.WhenAny(AttackTasks);
                AttackTasks.Remove(CharaterDead);
            }

            if (AttackTasks.Count == 1)
            {
                Console.WriteLine("Winner is : {0}", AttackTasks[0].Result.Name);
            }
            foreach (Character character in Characters)
            {
                Console.WriteLine("{0} PV restant : {1} PV", character.Name, character.CurrentLife);
            }

            Console.WriteLine("Finish");

        }


    }
}

