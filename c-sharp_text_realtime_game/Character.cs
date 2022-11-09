using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Xml.Linq;
using c_sharp_text_realtime_game;

namespace c_sharp_realtime_game
{
    public delegate void DeathEventHandlerDelegate(Object sender, DeathEventArgs e);

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
        List<Character> TempCharacters = new List<Character>();
        public event DeathEventHandlerDelegate DeadCharacterEvent;

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
            TempCharacters.AddRange(fightManager.Characters);
            TempCharacters.Remove(this);
        }



        public async Task<Character> Start()
        {
            foreach (Character character in TempCharacters)
            {
                character.DeadCharacterEvent += DeleteDeadCharacter;
            }

            while (CurrentLife > 0)
            {
                int delayBeforeNextAttack = (int)((1000 / AttackSpeed) - RollDice());
                await Task.Delay(delayBeforeNextAttack);

                if (CurrentLife > 0)
                {
                    Attack();

                    // I'm the last character
                    if (fightManager.Characters.Count == 1)
                    {
                        return this;
                    }
                }
            }

            // I'm Dead
            fightManager.Characters.Remove(this);

            foreach (Character character in TempCharacters)
            {
                character.DeadCharacterEvent -= DeleteDeadCharacter;
            }

            DeathEventArgs args = new DeathEventArgs();
            args.DeadCharacter = this;

            // Send my death event to all other characters (all listeners)
            DeadCharacterEvent?.Invoke(this, args);   // Invoke the delegate

            return this;
        }


        virtual public void SpecialSpell()
        {

        }

        virtual public void Passive()
        {

        }

        virtual public void Attack()
        {
            Character target = Target();

            if (target != null)
            {
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
                    Console.WriteLine("{0} : Echec de l'attaque !", this.Name);
                }
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

        // Event handler
        public void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            Console.WriteLine("{0} : {1} est mort", this.Name, e.DeadCharacter.Name);
            TempCharacters.Remove(e.DeadCharacter);
        }




        // Selectionner une cible valide
        public Character Target()
        {
            // On cree une liste dans laquelle on stockera les cibles valides
            List<Character> validTarget = new List<Character>();

            for (int i = 0; i < fightManager.Characters.Count; i++)
            {
                Character currentCharacter = fightManager.Characters[i];
                // Si le personnage testé n'est pas celui qui attaque et qu'il est vivant
                if (currentCharacter != this && currentCharacter.CurrentLife > 0)
                {
                    //on l'ajoute à la liste des cible valide
                    validTarget.Add(currentCharacter);
                }
            }

            if (validTarget.Count > 0)
            {
                // On prend un personnage au hasard dans la liste des cibles valides et on le designe comme la cible de l'attaque 
                Character target = validTarget[random.Next(0, validTarget.Count)];
                return target;
            }
            return null;
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

