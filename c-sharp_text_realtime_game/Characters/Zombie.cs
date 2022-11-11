using System;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Zombie : Undead
    {
        public Zombie(string name) : base(name, 150, 0, 1, 20, 1500, 1500, 0.1)
        {
        }

        public override Task<Task> DamageTakenDelayAttack()
        {
            return new Task<Task>(async () =>
            {
                DelayAttacks.Clear();
                await Task.Delay(0);
            });
        }

        public override void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            base.DeleteDeadCharacter(sender, e);
            EatDeadCharacter(e.DeadCharacter);
        }

        public void EatDeadCharacter(Character target)
        {
            Console.WriteLine("{0} : mange le cadavre de {1}", Name, target.Name);
            CurrentLife += target.MaximumLife;
        }
    }
}
