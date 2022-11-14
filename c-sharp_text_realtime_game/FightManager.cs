using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class FightManager
    {
        public List<Fight> Fights = new List<Fight>();
        public List<Fight> FightResults = new List<Fight>();

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
                fightTasks.Add(fight.Start());
            }

            while (fightTasks.Count > 0)
            {
                Task<Fight> fightFinished = await Task.WhenAny(fightTasks);
                fightTasks.Remove(fightFinished);

                FightResults.Add(fightFinished.Result);
            }

            Console.WriteLine("All Fight Are Finished !");
            Console.WriteLine();


            // Calcul du score global parmi tous les combats
            List<CharacterType> rankResults = new List<CharacterType>();

            foreach (Fight fightResult in FightResults)
            {
                foreach (CharacterType characterType in fightResult.RankCharacterTypes)
                {
                    if (!rankResults.Any(m => m.Type == characterType.Type))
                    {
                        rankResults.Add(new CharacterType(characterType.Type, characterType.NumberWin));
                    }
                    else
                    {
                        rankResults.Find(m => m.Type == characterType.Type).NumberWin += characterType.NumberWin;
                    }
                }
            }

            Console.WriteLine("Les meilleurs type de personnage sont :");

            foreach (CharacterType characterType in rankResults.OrderByDescending(x => x.NumberWin))
            {
                Console.WriteLine("{0} avec un score de victoire de : {1}", characterType.Type.Name, characterType.NumberWin);
            }
        }

    }



}

