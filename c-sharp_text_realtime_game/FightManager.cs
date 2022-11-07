using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace c_sharp_realtime_game
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

            while (Characters.Count > 1)
            {
                for (int i = Characters.Count - 1; i >= 0; i--)
                {
                    //on stocke le personnage dans une variable pour éviter d'accéder inutilement à la liste
                    Character currentPersonnage = Characters[i];
                    if (currentPersonnage.CurrentLife <= 0)
                    {
                        Characters.Remove(currentPersonnage);
                    }
                }
                if (Characters.Count == 1)
                {
                    Console.WriteLine("Winner is : {0}", Characters[0].Name);
                    return;
                }

                List<Task> AttackTasks = new List<Task>();
                foreach (Character character in Characters)
                {
                    Character target = character.Target();
                    AttackTasks.Add(character.Attack(target));
                }

                await Task.WhenAny(AttackTasks).Result;
            }

            Console.WriteLine("Finish");

        }


    }
}

