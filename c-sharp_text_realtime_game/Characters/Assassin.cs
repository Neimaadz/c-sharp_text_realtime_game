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
        ConsoleColor IPoisoning.Color { get => this.Color; set => this.Color = value; }
        string ICamouflage.Name { get => this.Name; set => this.Name = value; }
        bool ICamouflage.IsCamouflaged { get => IsCamouflaged; set => IsCamouflaged = value; }
        Timer ICamouflage.CamouflagedTimer { get => this.CamouflagedTimer; set => this.CamouflagedTimer = value; }
        List<Character> ICamouflage.Characters { get => this.FightManager.Characters; set => this.FightManager.Characters = value; }
        ConsoleColor ICamouflage.Color { get => this.Color; set => this.Color = value; }

        bool IsCamouflaged = false;
        Timer CamouflagedTimer = new Timer();

        public Assassin(string name) : base(name, 150, 100, 1, 100, 185, 185, 0.5, (ConsoleColor)2)
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
                    MyLog(this.Name + " perd son camouflage");
                    this.IsCamouflaged = false;
                }

                MyLog(this.Name + " : Attaque " + target.Name);

                int attackMarge = AttackMarge(target);

                if (attackMarge > 0)
                {
                    int damageDeal = attackMarge * this.DamageRate / 100;

                    // Coup Critique
                    if (damageDeal > (target.CurrentLife / 2))
                    {
                        MyLog(this.Name + " :" + " Coup critique !");
                        target.CurrentLife -= target.CurrentLife + damageDeal;
                    }
                    else
                    {
                        DealCommonDamage(target, damageDeal, 1);
                        (this as IPoisoning).DealPoisonDamage(target, damageDeal, 0.1);

                        target.DelayAttacks.Add(damageDeal);
                    }

                    MyLog(target.Name + " PV restant : " + target.CurrentLife + " PV");
                }
                else
                {
                    MyLog(this.Name + " : Echec de l'attaque !");
                }
            }

        }



        public override void RemainingCharacters(object sender, RemainingCharactersEventArgs e)
        {
            MyLog(this.Name + " :  Il reste 5 combattans en vie");
            if (this.IsCamouflaged)
            {
                MyLog(this.Name + " perd son camouflage");
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
                    MyLog(this.Name + " perd son camouflage");
                    this.IsCamouflaged = false;
                }

                MyLog(this.Name + " : Empoisonement");
                MyLog(this.Name + " : -" + poisonDamage + " PDV");

                this.CurrentLife -= poisonDamage;
            }
        }


    }
}
