using System;
using System.Threading.Tasks;

namespace c_sharp_text_realtime_game
{
    public class Warrior : Character
    {
        public Warrior(string name) : base (name, 150, 105, 2.2, 150, 250, 250, 0.2)
        {
        }

        /*
         * POUVOIR : augmente la vitesse d’attaque de 0,5 pendant 3 secondes.
         */
        public override async Task SpecialSpell()
        {
            //await IncreaseAttackSpeed();
        }

        public async Task IncreaseAttackSpeed()
        {
            Console.WriteLine("{0} : augmente sa vitesse d'attaque de 0,5", this.Name);
            this.AttackSpeed += 0.5;
            Console.WriteLine("{0} : vitesse d'attaque {1}", this.Name, this.AttackSpeed);
            await Task.Delay(3000);
            this.AttackSpeed -= 0.5;
        }
    }
}

