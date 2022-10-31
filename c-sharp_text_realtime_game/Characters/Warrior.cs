using System;
namespace c_sharp_realtime_game.Characters
{
    public class Warrior : Character
    {
        public Warrior(string name) : base (name, 150, 105, 2.2, 150, 250, 250, 0.2)
        {
        }

        public override void SpecialSpell()
        {
            this.AttackSpeed = 0.5;
        }
    }
}

