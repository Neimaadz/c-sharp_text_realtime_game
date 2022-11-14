using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace c_sharp_text_realtime_game
{
    public delegate void DeathEventHandlerDelegate(Object sender, DeathEventArgs e);
    public delegate void ReamainingCharactersEventHandlerDelegate(Object sender, RemainingCharactersEventArgs e);

    public class Character
    {
        public string Name;
        public int AttackRate;
        public int DefenseRate;
        public double AttackSpeed;
        public int DamageRate;
        public int MaximumLife;
        public int CurrentLife;
        public double PowerSpeed;
        public ConsoleColor Color { get; set; }

        public Random Random;
        public int RandomSeed;
        public Fight Fight;
        protected List<Character> TempCharacters = new List<Character>();
        public event DeathEventHandlerDelegate DeadCharacterEvent;
        public List<int> DelayAttacks = new List<int>();
        Timer PoisonTimer = new Timer();
        public List<int> PoisonDamages = new List<int>();
        public bool IsSpecialSpellAvailable = true;
        Timer SpecialSpellTimer = new Timer();
        public event ReamainingCharactersEventHandlerDelegate ReamainingCharactersEvent;

        public Character(string name, int attackRate, int defenseRate, double attackSpeed, int damageRate, int maximumLife, int currentLife, double powerSpeed, ConsoleColor color = ConsoleColor.White)
        {
            this.Name = name;
            this.AttackRate = attackRate;
            this.DefenseRate = defenseRate;
            this.AttackSpeed = attackSpeed;
            this.DamageRate = damageRate;
            this.MaximumLife = maximumLife;
            this.CurrentLife = currentLife;
            this.PowerSpeed = powerSpeed;
            this.Color = color;

            RandomSeed = NameToInt() + (int)DateTime.Now.Ticks;
            this.Random = new Random(RandomSeed);
            SpecialSpellTimer.Elapsed += SpecialSpellEvent;
        }

        public virtual void SetFight(Fight fight)
        {
            this.Fight = fight;
            TempCharacters.AddRange(fight.Characters);
            TempCharacters.Remove(this);
        }

        public async Task<Character> Start()
        {
            foreach (Character character in TempCharacters)
            {
                character.DeadCharacterEvent += DeleteDeadCharacter;
                character.ReamainingCharactersEvent += RemainingCharacters;
            }

            PoisonEventTimer(5000);

            while (this.CurrentLife > 0 && this.Fight.Characters.Count > 1)
            {
                await DamageTakenDelayTask();
                await DefaultAttackTask();
            }

            Dead();

            List<Character> remainingCharacters = this.Fight.Characters;

            // Il reste moins de 5 combattants en vie
            if (remainingCharacters.Count == 5)
            {
                RemainingCharactersEventArgs args = new RemainingCharactersEventArgs();
                args.RemainingCharacters = remainingCharacters;

                // Send remaining characters event to all listeners
                this.ReamainingCharactersEvent?.Invoke(this, args);
            }

            return this;
        }

        protected void Dead()
        {
            this.PoisonTimer.Enabled = false;
            foreach (Character character in TempCharacters)
            {
                character.DeadCharacterEvent -= DeleteDeadCharacter;
                character.ReamainingCharactersEvent -= RemainingCharacters;
                character.PoisonTimer.Elapsed -= PoisonEvent;
            }

            DeathEventArgs args = new DeathEventArgs();
            args.DeadCharacter = this;

            // Send my death event to all other characters (all listeners)
            this.DeadCharacterEvent?.Invoke(this, args);   // Invoke the delegate
        }

        virtual public void SpecialSpell()
        {
        }

        virtual public void Attack()
        {
            Character target = Target();

            if (target != null)
            {
                MyLog(this.Name + " : Attaque " + target.Name);

                int attackMarge = AttackMarge(target);

                if (attackMarge > 0)
                {
                    int damageDeal = attackMarge * this.DamageRate / 100;
                    DealCommonDamage(target, damageDeal, 1);

                    target.DelayAttacks.Add(damageDeal);

                    MyLog(target.Name + " PV restant : " +  target.CurrentLife + " PV");
                }
                else
                {
                    MyLog(this.Name + " : Echec de l'attaque !");
                }
            }
        }

        public virtual Task DamageTakenDelayTask()
        {
            Task<Task> damageTakenDelayAttack = new Task<Task>(async () =>
            {
                int delayAttack = 0;

                this.DelayAttacks.ForEach(delay =>
                {
                    delayAttack += delay;
                });
                await Task.Delay(delayAttack);
            }); ;

            return Task.Run(async () =>
            {
                damageTakenDelayAttack.Start(TaskScheduler.Default);
                await damageTakenDelayAttack.Unwrap();
            });
        }

        public Task DefaultAttackTask()
        {
            Task<Task> delayAttack = new Task<Task>(async () =>
            {
                await Task.Delay(DelayAttack(this.AttackSpeed));
            });
            
            return Task.Run(async () =>
            {
                delayAttack.Start(TaskScheduler.Default);
                await delayAttack.Unwrap();

                if (this.CurrentLife > 0)
                {
                    Attack();

                    if (this.IsSpecialSpellAvailable)
                    {
                        SpecialSpell();
                        SpecialSpellEventTimer();
                    }
                }
            });
        }
        public void SpecialSpellEventTimer()
        {
            SpecialSpellTimer.Enabled = false;
            SpecialSpellTimer.Interval = DelayAttack(this.PowerSpeed);
            SpecialSpellTimer.Enabled = true;
            this.IsSpecialSpellAvailable = false;
        }

        public void SpecialSpellEvent(object source, ElapsedEventArgs e)
        {
            this.IsSpecialSpellAvailable = true;
        }

        public void PoisonEventTimer (int interval)
        {
            this.PoisonTimer.Elapsed += PoisonEvent;
            this.PoisonTimer.Interval = interval;
            this.PoisonTimer.Enabled = true;
        }

        public virtual void PoisonEvent(object source, ElapsedEventArgs e)
        {
            int poisonDamage = 0;

            this.PoisonDamages.ForEach(poison =>
            {
                poisonDamage += poison;
            });

            if (poisonDamage > 0)
            {
                MyLog(this.Name + " : Empoisonement");
                MyLog(this.Name + " : -" + poisonDamage + " PDV");

                this.CurrentLife -= poisonDamage;
            }
        }

        // Event handler
        public virtual void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            MyLog(this.Name + " : " + e.DeadCharacter.Name + " est mort");

            // Delete the dead target
            this.Fight.Characters.Remove(e.DeadCharacter);
        }

        public virtual void RemainingCharacters(object sender, RemainingCharactersEventArgs e)
        {
            MyLog(this.Name + " :  Il reste 5 combattans en vie");
        }










        public void DealCommonDamage(Character target, int damageDeal, double rate)
        {
            int damage = (int)(damageDeal * rate);
            MyLog(target.Name + " : -" + damage + " PDV");

            target.CurrentLife -= damage;
        }

        // Selectionner une cible valide
        public virtual Character Target()
        {
            // On cree une liste dans laquelle on stockera les cibles valides
            List<Character> validTarget = new List<Character>();

            for (int i = 0; i < this.TempCharacters.Count; i++)
            {
                Character currentCharacter = this.TempCharacters[i];

                // Si le personnage testé n'est pas celui qui attaque et qu'il est vivant
                if (currentCharacter != this && currentCharacter.CurrentLife > 0)
                {
                    // Si le personnage NE POSSEDE PAS la capacité de se camoufler
                    if (!(currentCharacter is ICamouflage))
                    {
                        // On l'ajoute à la liste des cible valide
                        validTarget.Add(currentCharacter);
                    }
                    // le personnage POSSEDE la capacité de se camoufler
                    else
                    {
                        // Si le personnage n'est pas camouflé, il est donc une cible
                        if (!(currentCharacter as ICamouflage).IsCamouflaged)
                        {
                            validTarget.Add(currentCharacter);
                        }
                    }
                }
            }

            if (validTarget.Count > 0)
            {
                // On prend un personnage au hasard dans la liste des cibles valides et on le designe comme la cible de l'attaque 
                Character target = validTarget[this.Random.Next(0, validTarget.Count)];
                return target;
            }
            return null;
        }

        public int DelayAttack(double speed)
        {
            return (int)((1000 / speed) - RollDice());
        }

        protected int AttackMarge(Character target)
        {
            return AttackRoll() - target.DefenseRoll();
        }

        protected virtual int AttackRoll()
        {
            return this.AttackRate + RollDice();
        }

        protected virtual int DefenseRoll()
        {
            return this.DefenseRate + RollDice();
        }

        protected virtual int RollDice()
        {
            return this.Random.Next(1, 101);
        }

        private int NameToInt()
        {
            int result = 0;
            foreach (char c in Name)
            {
                result += c;
            }
            return result;
        }
        public void MyLog(string text)
        {
            Console.ForegroundColor = this.Color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void MyLog(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}

