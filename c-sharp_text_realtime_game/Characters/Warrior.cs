using System;
using System.Threading.Tasks;

namespace c_sharp_realtime_game.Characters
{
    public class Warrior : Character
    {
        readonly double CurrentAttackSpeed;

        public Warrior(string name) : base (name, 150, 105, 2.2, 350, 250, 250, 0.2)
        {
            CurrentAttackSpeed = AttackSpeed;
        }

        public async Task IncreaseAttackSpeed()
        {
            this.AttackSpeed += 0.5;
            await Task.Delay(3000);
            this.AttackSpeed = CurrentAttackSpeed;
        }
    }
}

