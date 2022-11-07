using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace c_sharp_realtime_game
{
    public class Character
    {
        public string Name;
        public int AttackRate;
        public int DefenseRate;
        public double AttackSpeed;
        public int DamageRate;
        public int MaximumLife;
        public int CurrentLife;
        public double PowerSpeed;

        public Random random;
        public int RandomSeed;
        public FightManager fightManager;

        public Character(string name, int attackRate, int defenseRate, double attackSpeed, int damageRate, int maximumLife, int currentLife, double powerSpeed)
        {
            this.Name = name;
            this.AttackRate = attackRate;
            this.DefenseRate = defenseRate;
            this.AttackSpeed = attackSpeed;
            this.DamageRate = damageRate;
            this.MaximumLife = maximumLife;
            this.CurrentLife = currentLife;
            this.PowerSpeed = powerSpeed;

            RandomSeed = NameToInt() + (int)DateTime.Now.Ticks;
            this.random = new Random(RandomSeed);
        }

        public void SetFightManager(FightManager fightManager)
        {
            this.fightManager = fightManager;
        }



        virtual public void SpecialSpell()
        {

        }

        virtual public void Passive()
        {

        }

        virtual public async Task Attack(Character target)
        {
            await NextAttackRoll();

            Console.WriteLine("{0} Attaque", this.Name, target.Name);

            int attackMarge = AttackMarge(target);

            if (attackMarge > 0)
            {
                int damageDeal = attackMarge * DamageRate / 100;

                Console.WriteLine("{0} : -{1} PV", target.Name, damageDeal);
                target.CurrentLife -= damageDeal;

                Console.WriteLine("{0} PV restant : {1} PV", target.Name, target.CurrentLife);
            }
            else
            {
                Console.WriteLine("Echec de l'attaque !");
            }
        }

        private int AttackMarge(Character target)
        {
            return target.DefenseRoll() - AttackRoll();
        }

        public int AttackRoll()
        {
            return AttackRate + RollDice();
        }
        public int DefenseRoll()
        {
            return DefenseRate + RollDice();
        }
        private async Task NextAttackRoll()
        {
            int delayBeforeNextAttack = (int)((1000 / AttackSpeed) - RollDice());
            await Task.WhenAny(new[] { Task.Delay(delayBeforeNextAttack) });
        }

        // Selectionner une cible valide
        public Character Target()
        {
            // On cree une liste dans laquelle on stockera les cibles valides
            List<Character> validTarget = new List<Character>();

            for (int i = 0; i < fightManager.Characters.Count; i++)
            {
                Character currentCharacter = fightManager.Characters[i];
                //si le personnage testé n'est pas celui qui attaque et qu'il est vivant
                if (currentCharacter != this && currentCharacter.CurrentLife > 0)
                {
                    //on l'ajoute à la liste des cible valide
                    validTarget.Add(currentCharacter);
                }
            }

            // On prend un personnage au hasard dans la liste des cibles valides et on le designe comme la cible de l'attaque 
            Character target = validTarget[random.Next(0, validTarget.Count)];
            return target;
        }

        private int RollDice()
        {
            return random.Next(1, 101);
        }

        private int NameToInt()
        {
            int result = 0;
            foreach (char c in Name)
            {
                result += c;
            }
            return result;
        }
    }
}

