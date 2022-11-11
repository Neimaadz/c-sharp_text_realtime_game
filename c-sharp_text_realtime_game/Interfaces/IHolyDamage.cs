using System;
namespace c_sharp_realtime_game.Interfaces
{
    public interface IHolyDamage
    {
        public string Name { get; set; }

        public void DealHolyDamage(Character target, int damageDeal)
        {
            if (target is Undead)
            {
                Console.WriteLine("{0} inflige des dégats sacrés", Name);
                Character.DealCommonDamage(target, damageDeal * 2); // dégats communs x2
            }
            else
            {
                Character.DealCommonDamage(target, damageDeal);
            }
        }
    }
}

