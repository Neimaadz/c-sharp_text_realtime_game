using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace c_sharp_text_realtime_game
{
    public class Robot :Character
    {
        public Robot(string name) : base(name, 25, 100, 1.2, 50, 275, 275, 0.5, (ConsoleColor)5)
        {
        }


        /*
         * POUVOIR : A chaque utilisation de son pouvoir, le robot augmente son attaque de 50%.
         */
        public override void SpecialSpell()
        {
            IncreaseAttackRate();
        }
        public void IncreaseAttackRate()
        {
            MyLog(this.Name + " augmente son attaque de 50%");
            this.AttackRate += (int)(this.AttackRate * 0.5);
        }
        public override void PoisonEvent(object source, ElapsedEventArgs e)
        {
            MyLog(this.Name + " est immunisé contre le poison");
        }

        /*
         * PASSIF : Les jets ne se font pas avec des nombres aléatoires entre 1 et 100.
         * Il suffit d’ajouter 50 à la caractéristique pour avoir le jet d’un robot. N’est pas vivant.
         * Le robot est immunisé contre le poison.
         */
        protected override int AttackRoll()
        {
            return this.AttackRate + 50;
        }

        protected override int DefenseRoll()
        {
            return this.DefenseRate + 50;
        }
    }
}
