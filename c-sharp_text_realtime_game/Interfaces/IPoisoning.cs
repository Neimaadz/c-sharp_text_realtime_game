using System;
namespace c_sharp_text_realtime_game.Interfaces
{
    public interface IPoisoning
    {
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }
        public void DealPoisonDamage(Character target, int damageDeal, double rate)
        {
            int poisonDamage = (int)(damageDeal * rate);
            Character.MyLog(this.Name + " empoisone " + target.Name, Color);

            target.PoisonDamages.Add(poisonDamage);
        }
    }
}

