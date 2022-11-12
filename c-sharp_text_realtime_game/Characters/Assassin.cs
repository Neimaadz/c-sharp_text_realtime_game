using c_sharp_text_realtime_game.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Assassin : Character, IPoisoning
    {
        string IPoisoning.Name { get => Name; set => Name = value; }
        public Assassin(string name) : base(name, 150, 100, 1, 100, 185, 185, 0.5)
        {
        }


        public override void SpecialSpell()
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
                    int damageDeal = attackMarge * this.DamageRate / 100;

                    if (damageDeal > (target.CurrentLife / 2))
                    {
                        Console.WriteLine("{0} Coup critique !", this.Name);
                        target.CurrentLife -= target.CurrentLife + damageDeal;
                    }
                    else
                    {
                        DealCommonDamage(target, damageDeal, 1);
                        (this as IPoisoning).DealPoisonDamage(target, damageDeal, 0.1);

                        target.DelayAttacks.Add(damageDeal);
                    }
                    Console.WriteLine("{0} PV restant : {1} PV", target.Name, target.CurrentLife);
                }
                else
                {
                    Console.WriteLine("{0} : Echec de l'attaque !", this.Name);
                }
            }

        }


    }
}
