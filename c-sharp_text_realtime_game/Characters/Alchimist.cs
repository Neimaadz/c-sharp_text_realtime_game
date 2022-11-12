using c_sharp_text_realtime_game.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace c_sharp_text_realtime_game
{
    public class Alchimist : Character, IHolyDamage, IPoisoning
    {
        string IHolyDamage.Name { get => Name; set => Name = value; }
        string IPoisoning.Name { get => Name; set => Name = value; }
        public Alchimist(string name) : base(name, 50, 50, 1, 30, 150, 150, 0.1)
        {
        }

        public override void SpecialSpell()
        {
            Character characterHighestCurrentLife = FightManager.Characters.OrderByDescending(x => x.CurrentLife).First();
            this.CurrentLife = characterHighestCurrentLife.CurrentLife;

            // Pour caper la vie
            if (this.CurrentLife >= this.MaximumLife)
            {
                this.CurrentLife = this.MaximumLife;
            }
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
                    (this as IHolyDamage).DealHolyDamage(target, damageDeal, 0.5);
                    (this as IPoisoning).DealPoisonDamage(target, damageDeal, 1);

                    target.DelayAttacks.Add(damageDeal);

                    Console.WriteLine("{0} PV restant : {1} PV", target.Name, target.CurrentLife);
                }
                else
                {
                    Console.WriteLine("{0} : Echec de l'attaque !", this.Name);
                }
            }
        }

        protected override int RollDice()
        {
            return this.Random.Next(1, 201);
        }

    }
}
