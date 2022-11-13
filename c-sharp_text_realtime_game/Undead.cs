using System;
using System.Timers;

namespace c_sharp_text_realtime_game
{
    public class Undead : Character
    {
        public Undead(string name, int attackRate, int defenseRate, double attackSpeed, int damageRate, int maximumLife, int currentLife, double powerSpeed, ConsoleColor color = ConsoleColor.White) :
            base(name, attackRate, defenseRate, attackSpeed, damageRate, maximumLife, currentLife, powerSpeed, color)
        {
            this.Name = name;
            this.AttackRate = attackRate;
            this.DefenseRate = defenseRate;
            this.AttackSpeed = attackSpeed;
            this.DamageRate = damageRate;
            this.MaximumLife = maximumLife;
            this.CurrentLife = currentLife;
            this.PowerSpeed = powerSpeed;
            this.Color = color;
        }

        public override void PoisonEvent(object source, ElapsedEventArgs e)
        {
            MyLog(this.Name + " est immunisé contre le poison");
        }
    }
}

