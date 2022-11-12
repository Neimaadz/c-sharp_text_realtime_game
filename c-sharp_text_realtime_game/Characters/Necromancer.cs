using c_sharp_text_realtime_game.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace c_sharp_text_realtime_game
{
    public class Necromancer : Undead, IPoisoning
    {
        string IPoisoning.Name { get => Name; set => Name = value; }

        public Necromancer(string name) : base(name, 0, 10, 1, 0, 275, 275, 5)
        {
        }

        public override void Attack()
        {
            Character target = Target();

            if (target != null)
            {
                Console.WriteLine("{0} Attaque", this.Name, target.Name);

                int attackMarge = AttackMarge(target);

                if (attackMarge > 0)
                {
                    int damageDeal = attackMarge * DamageRate / 100;
                    DealCommonDamage(target, damageDeal, 0.5);
                    (this as IPoisoning).DealPoisonDamage(target, damageDeal, 0.5);

                    target.DelayAttacks.Add(damageDeal);

                    Console.WriteLine("{0} PV restant : {1} PV", target.Name, target.CurrentLife);
                }
                else
                {
                    Console.WriteLine("{0} : Echec de l'attaque !", this.Name);
                }
            }
        }
        public override void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            base.DeleteDeadCharacter(sender, e);
            Console.WriteLine("{0} : renforcement !", this.Name);
            this.AttackRate += 5;
            this.DefenseRate += 5;
            this.DamageRate += 5;
            this.CurrentLife += 50;
            this.MaximumLife += 50;
        }

        protected override int RollDice()
        {
            return this.Random.Next(1, 151);
        }
    }
}
