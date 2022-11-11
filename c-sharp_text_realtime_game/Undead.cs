using System;
namespace c_sharp_realtime_game
{
    public class Undead : Character
    {
        public Undead(string name, int attackRate, int defenseRate, double attackSpeed, int damageRate, int maximumLife, int currentLife, double powerSpeed) :
            base(name, attackRate, defenseRate, attackSpeed, damageRate, maximumLife, currentLife, powerSpeed)
        {
            this.Name = name;
            this.AttackRate = attackRate;
            this.DefenseRate = defenseRate;
            this.AttackSpeed = attackSpeed;
            this.DamageRate = damageRate;
            this.MaximumLife = maximumLife;
            this.CurrentLife = currentLife;
            this.PowerSpeed = powerSpeed;
        }
    }
}

