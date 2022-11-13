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
        string ICamouflage.Name { get => this.Name; set => this.Name = value; }
        bool ICamouflage.IsCamouflaged { get => IsCamouflaged; set => IsCamouflaged = value; }
        Timer ICamouflage.CamouflagedTimer { get => this.CamouflagedTimer; set => this.CamouflagedTimer = value; }
        List<Character> ICamouflage.Characters { get => this.FightManager.Characters; set => this.FightManager.Characters = value; }

        bool IsCamouflaged = false;
        Timer CamouflagedTimer = new Timer();
        int CharactersNumber;

        public Necromancer(string name) : base(name, 0, 10, 1, 0, 275, 275, 5)
        {
        }

        public override void SetFightManager(FightManager fightManager)
        {
            base.SetFightManager(fightManager);
            this.CharactersNumber = this.FightManager.Characters.Count;
        }


        /*
         * POUVOIR : Tant qu’aucun combattant n’est mort, ce pouvoir permet au nécromancien de se
         * camoufler. Dès qu’un combattant est mort le pouvoir ne fonctionne plus.
         */
        public override void SpecialSpell()
        {
            if (this.FightManager.Characters.Count == CharactersNumber)
            {
                (this as ICamouflage).Camouflage();
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

            CamouflagedTimer.Enabled = false;
            CamouflagedTimer.Elapsed -= (this as ICamouflage).CamouflagedEvent;
            if (IsCamouflaged)
            {
                Console.WriteLine("{0} perd son camouflage", this.Name);
                IsCamouflaged = false;
            }
        }



        protected override int RollDice()
        {
            return this.Random.Next(1, 151);
        }
    }
}
