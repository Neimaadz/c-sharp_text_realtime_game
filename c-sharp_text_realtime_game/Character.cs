using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace c_sharp_realtime_game
{
    public class Character
    {
        public string Name;
        protected int AttackRate;
        protected int DefenseRate;
        protected double AttackSpeed;
        protected int DamageRate;
        protected int MaximumLife;
        protected int CurrentLife;
        protected double PowerSpeed;

        public Random random;
        public int RandomSeed;

        public Character(string name, int attackRate, int defenseRate, double attackSpeed, int damageRate, int maximumLife, int currentLife, double powerSpeed)
        {
            RandomSeed = NameToInt() + (int)DateTime.Now.Ticks;
            this.random = new Random(RandomSeed);

            this.Name = name;
            this.AttackRate = attackRate;
            this.DefenseRate = defenseRate;
            this.AttackSpeed = attackSpeed;
            this.DamageRate = damageRate;
            this.MaximumLife = maximumLife;
            this.CurrentLife = currentLife;
            this.PowerSpeed = powerSpeed;
        }



        virtual public void SpecialSpell()
        {

        }

        virtual public void Passive()
        {

        }

        virtual public void Attack(Character target)
        {
            int attackMarge = AttackMarge(target);

            if (attackMarge > 0)
            {
                target.CurrentLife = attackMarge * DamageRate / 100;
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
        public int NextAttackRoll()
        {
            return (int)((1000 / AttackSpeed) - RollDice());
        }

        public Character Target(List<Character> Characters)
        {
            List<Character> possibleTarget = Characters;
            possibleTarget.Remove(this);

            return possibleTarget[random.Next(0, possibleTarget.Count)];
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

