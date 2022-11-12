using c_sharp_text_realtime_game.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Vampire : Character
    {
        private int OldCurrentLife = 0;
        public Vampire(string name) : base(name, 125, 125, 2, 50, 150, 150, 0.2)
        {
            this.OldCurrentLife = this.CurrentLife;
        }


        /*
         * POUVOIR : Sélectionne une cible aléatoire et augmente son délai d’attaque du montant de dégâts que
         * le vampire a reçu depuis la dernière utilisation de son pouvoir.
         */
        public override void SpecialSpell()
        {
            IncreaseTargetDelayAttack();
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
                    DealCommonDamage(target, damageDeal, 1);

                    StealLife(target, damageDeal);

                    target.DelayAttacks.Add(damageDeal);

                    Console.WriteLine("{0} PV restant : {1} PV", target.Name, target.CurrentLife);
                }
                else
                {
                    Console.WriteLine("{0} : Echec de l'attaque !", this.Name);
                }
            }
        }

        public void IncreaseTargetDelayAttack ()
        {
            Character target = this.Target();
            int lifePointLost = this.OldCurrentLife - this.CurrentLife;

            Console.WriteLine("{0} augmente de {1} le delai d'attaque de {2}", this.Name, lifePointLost, target.Name);
            target.DelayAttacks.Add(lifePointLost);

            this.OldCurrentLife = this.CurrentLife;
        }

        /*
         * PASSIF : Se soigne d’un montant de vie équivalent à 50% des dégâts infligés en attaquant.
         * Ne fonctionne que sur les vivants.
         */
        private void StealLife (Character target, int damageDeal)
        {
            if (target is IAlive)
            {
                int damageSteal = (int)(damageDeal * 0.5);
                this.CurrentLife += damageSteal;
                Console.WriteLine("{0} vol de vie : +{1} PV", this.Name, damageSteal);

                // Pour caper la vie
                if (this.CurrentLife >= this.MaximumLife)
                {
                    this.CurrentLife = this.MaximumLife;
                }
            }
        }
    }
}
