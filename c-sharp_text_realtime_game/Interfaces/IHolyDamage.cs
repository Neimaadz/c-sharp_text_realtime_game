using System;
namespace c_sharp_text_realtime_game.Interfaces
{
    public interface IHolyDamage
    {
        public string Name { get; set; }
        public ConsoleColor Color { get; set; }

        public void DealHolyDamage(Character target, int damageDeal, double rate)
        {
            if (target is Undead)
            {
                Character.MyLog(this.Name + " inflige des dégats sacrés", Color);
                int damage = (int)(damageDeal * 2 * rate);
                Character.MyLog(target.Name + " : -" + damage + " PDV", Color);

                target.CurrentLife -= damage;
            }
            else
            {
                int damage = (int)(damageDeal * rate);
                Character.MyLog(target.Name + " : -" + damage + " PDV", Color);

                target.CurrentLife -= damage;
            }
        }
    }
}

