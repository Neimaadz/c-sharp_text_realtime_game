using System;
namespace c_sharp_realtime_game
{
    public class Character
    {
        public string Name;
        private int Attack;
        private int Defense;
        private double AttackSpeed;
        private int Damage;
        private int MaximumLife;
        private int CurrentLife;
        private double PowerSpeed;

        public Random random;
        public int RandomSeed;

        public Character(string name, int attack, int defense, double attackSpeed, int damage, int maximumLife, int currentLife, double powerSpeed)
        {
            RandomSeed = NameToInt() + (int)DateTime.Now.Ticks;
            this.random = new Random(RandomSeed);

            this.Attack = attack;
            this.Defense = defense;
            this.AttackSpeed = attackSpeed;
            this.Damage = damage;
            this.MaximumLife = maximumLife;
            this.CurrentLife = currentLife;
            this.PowerSpeed = powerSpeed;
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

