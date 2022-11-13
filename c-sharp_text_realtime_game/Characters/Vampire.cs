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
        public Vampire(string name) : base(name, 125, 125, 2, 50, 150, 150, 0.2, (ConsoleColor)6)
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
                MyLog(this.Name + " : Attaque " + target.Name);

                int attackMarge = AttackMarge(target);

                if (attackMarge > 0)
                {
                    int damageDeal = attackMarge * this.DamageRate / 100;
                    DealCommonDamage(target, damageDeal, 1);

                    StealLife(target, damageDeal);

                    target.DelayAttacks.Add(damageDeal);

                    MyLog(target.Name + " PV restant : " + target.CurrentLife + " PV");
                }
                else
                {
                    MyLog(this.Name + " : Echec de l'attaque !");
                }
            }
        }

        public void IncreaseTargetDelayAttack ()
        {
            Character target = this.Target();
            int lifePointLost = this.OldCurrentLife - this.CurrentLife;

            MyLog(this.Name + " augmente de " + lifePointLost + " le delai d'attaque de " + target.Name);
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
                MyLog(this.Name + " vol de vie : +" + damageSteal + " PV");

                // Pour caper la vie
                if (this.CurrentLife >= this.MaximumLife)
                {
                    this.CurrentLife = this.MaximumLife;
                }
            }
        }
    }
}
