using System;
using System.Timers;

namespace c_sharp_text_realtime_game
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

        public override void PoisonEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("{0} est immunisé contre le poison", this.Name);
        }
    }
}

