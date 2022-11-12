using System;
namespace c_sharp_text_realtime_game.Interfaces
{
    public interface IPoisoning
    {
        public string Name { get; set; }
        public void DealPoisonDamage(Character target, int damageDeal, double rate)
        {
            int poisonDamage = (int)(damageDeal * rate);
            Console.WriteLine("{0} : empoisone {1}", this.Name, target.Name);
            Console.WriteLine("{0} : -{1} PDV", target.Name, poisonDamage);

            target.PoisonDamages.Add(poisonDamage);
        }
    }
}

