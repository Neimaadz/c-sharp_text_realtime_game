using System;
using System.Threading.Tasks;
using System.Timers;

namespace c_sharp_text_realtime_game
{
    public class Warrior : Character, IAlive
    {
        Timer IncreaseAttackSpeedTimer = new Timer();

        public Warrior(string name) : base (name, 150, 105, 2.2, 150, 250, 250, 0.2, (ConsoleColor)7)
        {
        }


        /*
         * POUVOIR : augmente la vitesse d’attaque de 0,5 pendant 3 secondes.
         */
        public override void SpecialSpell()
        {
            IncreaseAttackSpeed();
        }

        public void IncreaseAttackSpeed()
        {
            MyLog(this.Name + " augmente sa vitesse d'attaque de 0,5");
            this.AttackSpeed += 0.5;
            IncreaseAttackSpeedEventTimer();
        }
        public void IncreaseAttackSpeedEventTimer()
        {
            IncreaseAttackSpeedTimer.Elapsed += IncreaseAttackSpeedEvent;
            IncreaseAttackSpeedTimer.Interval = 3000;
            IncreaseAttackSpeedTimer.Enabled = true;
        }
        private void IncreaseAttackSpeedEvent(object source, ElapsedEventArgs e)
        {
            IncreaseAttackSpeedTimer.Enabled = false;
            IncreaseAttackSpeedTimer.Elapsed -= IncreaseAttackSpeedEvent;
            this.AttackSpeed -= 0.5;
        }
    }
}

