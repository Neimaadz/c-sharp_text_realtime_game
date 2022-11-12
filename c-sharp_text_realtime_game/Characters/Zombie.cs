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
        public override void SpecialSpell()
        {
            if (this.ToEatDeadCharacters.Count > 0)
            {
                Character characterToEat = ToEatDeadCharacters[Random.Next(0, this.ToEatDeadCharacters.Count)];
                EatDeadCharacter(characterToEat);
            }
        }

        /*
         * PASSIF : Ne subit jamais de délai d’attaque. Jet de défense toujours égal à 0
         */
        public override Task DamageTakenDelayTask()
        {
            Task<Task> damageTakenDelayAttack = new Task<Task>(async () =>
            {
                this.DelayAttacks.Clear();
                await Task.Delay(0);
            }); ;

            return Task.Run(async () =>
            {
                damageTakenDelayAttack.Start(TaskScheduler.Default);
                await damageTakenDelayAttack.Unwrap();
            });
        }

        public override void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            base.DeleteDeadCharacter(sender, e);
            this.ToEatDeadCharacters.Add(e.DeadCharacter);
        }

        public void EatDeadCharacter(Character target)
        {
            Console.WriteLine("{0} : mange le cadavre de {1}", this.Name, target.Name);
            this.CurrentLife += target.MaximumLife;
            this.ToEatDeadCharacters.Remove(target);
        }
    }
}
