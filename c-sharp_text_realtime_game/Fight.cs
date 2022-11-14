using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Fight
    {
        public List<Character> Characters;
        public Character Winner;

        public Fight(List<Character> characters)
        {
            this.Characters = characters;
            foreach (Character character in Characters)
            {
                character.SetFight(this);
            }
        }



        public async Task<Fight> StartBattleRoyal()
        {
            Console.WriteLine("Starting Battle Royal");

            List<Task<Character>> attackTasks = new List<Task<Character>>();
            foreach (Character character in Characters)
            {
                attackTasks.Add(character.Start());
            }

            while (attackTasks.Count > 1)
            {
                Task<Character> charaterDead = await Task.WhenAny(attackTasks);
                attackTasks.Remove(charaterDead);
            }

            if (attackTasks.Count == 1)
            {
                foreach (Task<Character> winner in attackTasks)
                {
                    this.Winner = winner.Result;
                }
            }
            Console.WriteLine("Winner is : {0}", this.Winner.Name);
            Console.WriteLine("{0} PV restant : {1} PV", this.Winner.Name, this.Winner.CurrentLife);

            Console.WriteLine("Finish");

            return this;
        }
    }
}

