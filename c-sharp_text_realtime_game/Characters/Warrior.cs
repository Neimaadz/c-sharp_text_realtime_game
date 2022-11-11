using System;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Warrior : Character, IAlive
    {
        public Warrior(string name) : base (name, 150, 105, 2.2, 150, 250, 250, 0.2)
        {
        }


        /*
         * POUVOIR : augmente la vitesse d’attaque de 0,5 pendant 3 secondes.
         */
        public override async Task SpecialSpell()
        {
            await IncreaseAttackSpeed();
        }

        public Task IncreaseAttackSpeed()
        {
            _ = Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("{0} : augmente sa vitesse d'attaque de 0,5", Name);
                AttackSpeed += 0.5;
                Console.WriteLine("{0} : vitesse d'attaque {1}", Name, AttackSpeed);
                await Task.Delay(3000);
                AttackSpeed -= 0.5;
            }).ConfigureAwait(false);
            return Task.CompletedTask;
        }
    }
}

