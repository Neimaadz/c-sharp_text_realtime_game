using c_sharp_text_realtime_game.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace c_sharp_text_realtime_game
{
    public class Assassin : Character, IPoisoning, ICamouflage
    {
        string IPoisoning.Name { get => this.Name; set => this.Name = value; }
        string ICamouflage.Name { get => this.Name; set => this.Name = value; }
        bool ICamouflage.IsCamouflaged { get => IsCamouflaged; set => IsCamouflaged = value; }
        Timer ICamouflage.CamouflagedTimer { get => this.CamouflagedTimer; set => this.CamouflagedTimer = value; }
        List<Character> ICamouflage.Characters { get => this.FightManager.Characters; set => this.FightManager.Characters = value; }

        bool IsCamouflaged = false;
        Timer CamouflagedTimer = new Timer();

        public Assassin(string name) : base(name, 150, 100, 1, 100, 185, 185, 0.5)
        {
        }



        public override void SpecialSpell()
        {
            (this as ICamouflage).Camouflage();
        }

        public override void Attack()
        {
            Character target = Target();

            if (target != null)
            {
                if (this.IsCamouflaged)
                {
                    Console.WriteLine("{0} perd son camouflage", this.Name);
                    this.IsCamouflaged = false;
                }

                Console.WriteLine("{0} Attaque", this.Name, target.Name);

                int attackMarge = AttackMarge(target);

                if (attackMarge > 0)
                {
                    int damageDeal = attackMarge * this.DamageRate / 100;

                    // Coup Critique
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



        public override void RemainingCharacters(object sender, RemainingCharactersEventArgs e)
        {
            Console.WriteLine("{0} : Il reste 5 combattans en vie", this.Name);
            if (this.IsCamouflaged)
            {
                Console.WriteLine("{0} perd son camouflage", this.Name);
                this.IsCamouflaged = false;
            }
        }

        public override void PoisonEvent(object source, ElapsedEventArgs e)
        {
            int poisonDamage = 0;

            this.PoisonDamages.ForEach(poison =>
            {
                poisonDamage += poison;
            });

            if (poisonDamage > 0)
            {
                if (this.IsCamouflaged)
                {
                    Console.WriteLine("{0} perd son camouflage", this.Name);
                    this.IsCamouflaged = false;
                }

                Console.WriteLine("{0} : empoisonnement", this.Name);
                Console.WriteLine("{0} : -{1} PDV", this.Name, poisonDamage);

                this.CurrentLife -= poisonDamage;
            }
        }


    }
}
