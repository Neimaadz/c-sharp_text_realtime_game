using c_sharp_realtime_game;
using System;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game.Characters
{
    public class Zombie : Undead
    {
        public Zombie(string name) : base(name, 150, 0, 1, 20, 1500, 1500, 0.1)
        {
        }

        public override Task<Task> DamageTakenDelayAttack()
        {
            return null;
        }
        public void EatDeadCharacter(Character target)
        {
            Console.WriteLine("{0} : mange le cadavre de {1}", Name, target.Name);
            CurrentLife += target.MaximumLife;
        }
    }
}
