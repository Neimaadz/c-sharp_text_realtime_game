using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class FightManager
    {
        public List<Fight> Fights;

        public FightManager(List<Fight> fights)
        {
            this.Fights = fights;
        }

        public async Task StartMultipleFight()
        {
            Console.WriteLine("Starting Multiple Fight");

            List<Task<Fight>> fightTasks = new List<Task<Fight>>();
            foreach (Fight fight in Fights)
            {
                fightTasks.Add(fight.StartBattleRoyal());
            }

            while (fightTasks.Count > 0)
            {
                Task<Fight> fightFinished = await Task.WhenAny(fightTasks);
                fightTasks.Remove(fightFinished);
            }

            //await Task.WhenAll(fightTasks);

            Console.WriteLine("All Fight Are Finished !");
        }


    }
}

