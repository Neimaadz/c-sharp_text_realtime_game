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
            int currentLife = this.CurrentLife;
            this.CurrentLife = characterHighestCurrentLife.CurrentLife;
            characterHighestCurrentLife.CurrentLife = currentLife;

            Console.WriteLine("{0} Echange sa vie avec {1}", this.Name, characterHighestCurrentLife.Name);

            // Pour caper la vie
            if (this.CurrentLife >= this.MaximumLife)
            {
                this.CurrentLife = this.MaximumLife;
            }
        }

        public override void Attack()
        {
            List<Character> CharacterTargets = Targets();

            if (CharacterTargets.Count > 0)
            {
                foreach (Character target in CharacterTargets)
                {
                    Console.WriteLine("{0} Attaque", this.Name, target.Name);

                    int attackMarge = AttackMarge(target);

                    if (attackMarge > 0)
                    {
                        if (target is ICamouflage && (target as ICamouflage).IsCamouflaged)
                        {
                            Console.WriteLine("{0} perd son camouflage", target.Name);
                            (target as ICamouflage).IsCamouflaged = false;
                        }
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
        }

        private List<Character> Targets()
        {
            List<Character> targets = new List<Character>();   // Rénitialiser la liste des targets
            List<Character> validTarget = new List<Character>();

            for (int i = 0; i < this.TempCharacters.Count; i++)
            {
                Character currentCharacter = this.TempCharacters[i];
                if (currentCharacter != this && currentCharacter.CurrentLife > 0)
                {
                    validTarget.Add(currentCharacter);
                }
            }
            

            if (validTarget.Count > 0)
            {
                for (int i = 0; i < validTarget.Count; i++)
                {
                    // 50% de chances
                    if (Random.Next(2) == 1)
                    {
                        Character target = validTarget[i];
                        targets.Add(target);
                    }
                }
                return targets;
            }
            return targets;
        }

        protected override int RollDice()
        {
            return this.Random.Next(1, 201);
        }

    }
}
