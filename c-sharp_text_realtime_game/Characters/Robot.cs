using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Robot :Character, INotAlive
    {
        public Robot(string name) : base(name, 25, 100, 1.2, 50, 275, 275, 0.5)
        {
        }


        /*
         * POUVOIR : A chaque utilisation de son pouvoir, le robot augmente son attaque de 50%.
         */
        public override Task SpecialSpell()
        {
            IncreaseAttackRate();
            return Task.CompletedTask;
        }
        public void IncreaseAttackRate()
        {
            Console.WriteLine("{0} : augmente son attaque de 50%", Name);
            AttackRate += (int)(AttackRate * 0.5);
            Console.WriteLine("{0} : taux d'attaque {1}", Name, AttackRate);
        }

        /*
         * PASSIF : Les jets ne se font pas avec des nombres aléatoires entre 1 et 100.
         * Il suffit d’ajouter 50 à la caractéristique pour avoir le jet d’un robot. N’est pas vivant.
         * Le robot est immunisé contre le poison.
         */
        protected override int AttackRoll()
        {
            return AttackRate + 50;
        }

        protected override int DefenseRoll()
        {
            return DefenseRate + 50;
        }
    }
}
