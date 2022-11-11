using System;
namespace c_sharp_realtime_game.Interfaces
{
    public interface IPoisoning
    {
        public void PoisonDamage(Character target)
        {
            if (!(target is Undead))
            {
                // target.PoisonRates.Add(10);
            }
        }
    }
}

