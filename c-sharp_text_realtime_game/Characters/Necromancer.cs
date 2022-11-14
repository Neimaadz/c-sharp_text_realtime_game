using c_sharp_text_realtime_game.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace c_sharp_text_realtime_game
{
    public class Necromancer : Undead, IPoisoning, ICamouflage
    {
        string IPoisoning.Name { get => Name; set => Name = value; }
        ConsoleColor IPoisoning.Color { get => this.Color; set => this.Color = value; }
        string ICamouflage.Name { get => this.Name; set => this.Name = value; }
        bool ICamouflage.IsCamouflaged { get => IsCamouflaged; set => IsCamouflaged = value; }
        Timer ICamouflage.CamouflagedTimer { get => this.CamouflagedTimer; set => this.CamouflagedTimer = value; }
        List<Character> ICamouflage.Characters { get => this.Fight.Characters; set => this.Fight.Characters = value; }
        ConsoleColor ICamouflage.Color { get => this.Color; set => this.Color = value; }

        bool IsCamouflaged = false;
        Timer CamouflagedTimer = new Timer();
        int CharactersNumber;

        public Necromancer(string name) : base(name, 0, 10, 1, 0, 275, 275, 5, (ConsoleColor)3)
        {
        }


        public override void SetFight(Fight fight)
        {
            base.SetFight(fight);
            this.CharactersNumber = this.Fight.Characters.Count;
        }


        /*
         * POUVOIR : Tant qu’aucun combattant n’est mort, ce pouvoir permet au nécromancien de se
         * camoufler. Dès qu’un combattant est mort le pouvoir ne fonctionne plus.
         */
        public override void SpecialSpell()
        {
            if (this.Fight.Characters.Count == CharactersNumber)
            {
                (this as ICamouflage).Camouflage();
            }
        }

        public override void Attack()
        {
            Character target = Target();

            if (target != null)
            {
                MyLog(this.Name + " Attaque " + target.Name);

                int attackMarge = AttackMarge(target);

                if (attackMarge > 0)
                {
                    int damageDeal = attackMarge * DamageRate / 100;
                    DealCommonDamage(target, damageDeal, 0.5);
                    (this as IPoisoning).DealPoisonDamage(target, damageDeal, 0.5);

                    target.DelayAttacks.Add(damageDeal);

                    MyLog(target.Name + " PV restant : " + target.CurrentLife + " PV");
                }
                else
                {
                    MyLog(this.Name + " : Echec de l'attaque !");
                }
            }
        }
        public override void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            base.DeleteDeadCharacter(sender, e);
            MyLog(this.Name + " : Renforcement ");
            this.AttackRate += 5;
            this.DefenseRate += 5;
            this.DamageRate += 5;
            this.CurrentLife += 50;
            this.MaximumLife += 50;

            CamouflagedTimer.Enabled = false;
            CamouflagedTimer.Elapsed -= (this as ICamouflage).CamouflagedEvent;
            if (IsCamouflaged)
            {
                MyLog(this.Name + " perd son camouflage");
                IsCamouflaged = false;
            }
        }



        protected override int RollDice()
        {
            return this.Random.Next(1, 151);
        }
    }
}
