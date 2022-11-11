using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Zombie : Undead
    {
        private List<Character> ToEatDeadCharacters = new List<Character>();
        public Zombie(string name) : base(name, 150, 0, 1, 20, 1500, 1500, 0.1)
        {
        }


        /*
         * POUVOIR : Dévore l’un des cadavres sur le champ de bataille. Se soigne de maximumLife du
         * cadavre choisi. Un cadavre ne peut être dévoré qu’une fois
         */
        public override Task SpecialSpell()
        {
            if (ToEatDeadCharacters.Count > 0)
            {
                Character characterToEat = ToEatDeadCharacters[Random.Next(0, ToEatDeadCharacters.Count)];
                EatDeadCharacter(characterToEat);
            }
            return Task.CompletedTask;
        }

        /*
         * PASSIF : Ne subit jamais de délai d’attaque. Jet de défense toujours égal à 0
         */
        public override Task<Task> DamageTakenDelayAttack()
        {
            return new Task<Task>(async () =>
            {
                DelayAttacks.Clear();
                await Task.Delay(0);
            });
        }

        public override void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            base.DeleteDeadCharacter(sender, e);
            ToEatDeadCharacters.Add(e.DeadCharacter);
        }

        public void EatDeadCharacter(Character target)
        {
            Console.WriteLine("{0} : mange le cadavre de {1}", Name, target.Name);
            CurrentLife += target.MaximumLife;
            ToEatDeadCharacters.Remove(target);
        }
    }
}
