using c_sharp_text_realtime_game.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Priest : Character, IAlive, IHolyDamage
    {
        string IHolyDamage.Name { get => Name; set => Name = value; }
        ConsoleColor IHolyDamage.Color { get => this.Color; set => this.Color = value; }
        public Priest(string name) : base(name, 100, 125, 1.5, 90, 150, 150, 1, (ConsoleColor)4)
        {
        }



        /*
         * POUVOIR :se soigne de 10% de sa vie maximum
         */
        public override void SpecialSpell()
        {
            Heal();
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
                    int damageDeal = attackMarge * DamageRate / 100;
                    (this as IHolyDamage).DealHolyDamage(target, damageDeal, 1);

                    target.DelayAttacks.Add(damageDeal);

                    MyLog(target.Name + " PV restant : " + target.CurrentLife + " PV");
                }
                else
                {
                    MyLog(this.Name + " : Echec de l'attaque !");
                }
            }
        }

        private void Heal()
        {
            int heal = (int)(this.MaximumLife * 0.1);
            this.CurrentLife += heal;
            MyLog(this.Name + " se soigne");
            MyLog(this.Name + " : +" + heal + " PDV");

            // Pour caper la vie
            if (this.CurrentLife >= this.MaximumLife)
            {
                this.CurrentLife = this.MaximumLife;
            }
        }

        /*
         * PASSIF : Inflige des dégâts sacrés. S’il y a des morts-vivants parmi les combattants,
         * le prêtre les cible en priorité.
         */
        public override Character Target()
        {
            List<Character> validTarget = new List<Character>();
            List<Character> undeadCharacters = new List<Character>();

            for (int i = 0; i < this.Fight.Characters.Count; i++)
            {
                Character currentCharacter = this.Fight.Characters[i];

                // Si le personnage testé n'est pas celui qui attaque et qu'il est vivant
                if (currentCharacter != this && currentCharacter.CurrentLife > 0)
                {
                    // Si le personnage NE POSSEDE PAS la capacité de se camoufler
                    if (!(currentCharacter is ICamouflage))
                    {
                        // On l'ajoute à la liste des cible valide
                        validTarget.Add(currentCharacter);
                    }
                    // le personnage POSSEDE la capacité de se camoufler
                    else
                    {
                        // Si le personnage n'est pas camouflé, il est donc une cible
                        if (!(currentCharacter as ICamouflage).IsCamouflaged)
                        {
                            validTarget.Add(currentCharacter);
                        }
                    }
                }
            }

            if (validTarget.Count > 0)
            {
                for (int i = 0; i < validTarget.Count; i++)
                {
                    // S'il y a un Mort-vivant parmi la liste des cibles valides
                    if (validTarget[i] is Undead)
                    {
                        // On l'ajoute dans la liste
                        undeadCharacters.Add(validTarget[i]);
                    }
                }

                if (undeadCharacters.Count > 0)
                {
                    Character target = undeadCharacters[this.Random.Next(0, undeadCharacters.Count)];
                    return target;
                }
                else
                {
                    // On prend un personnage au hasard dans la liste des cibles valides et on le designe comme la cible de l'attaque 
                    Character target = validTarget[this.Random.Next(0, validTarget.Count)];
                    return target;
                }
            }
            return null;
        }
    }
}
