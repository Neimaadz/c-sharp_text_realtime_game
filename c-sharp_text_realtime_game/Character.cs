using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace c_sharp_text_realtime_game
{
    public delegate void DeathEventHandlerDelegate(Object sender, DeathEventArgs e);

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

        public Random Random;
        public int RandomSeed;
        public FightManager FightManager;
        List<Character> TempCharacters = new List<Character>();
        public event DeathEventHandlerDelegate DeadCharacterEvent;
        public List<int> DelayAttacks = new List<int>();
        public List<int> PoisonDamages = new List<int>();
        public bool IsSpecialSpellAvailable = true;
        Timer SpecialSpellTimer = new Timer();

        public Character(string name, int attackRate, int defenseRate, double attackSpeed, int damageRate, int maximumLife, int currentLife, double powerSpeed)
        {
            this.Name = name;
            this.AttackRate = attackRate;
            this.DefenseRate = defenseRate;
            this.AttackSpeed = attackSpeed;
            this.DamageRate = damageRate;
            this.MaximumLife = maximumLife;
            this.CurrentLife = currentLife;
            this.PowerSpeed = powerSpeed;

            RandomSeed = NameToInt() + (int)DateTime.Now.Ticks;
            this.Random = new Random(RandomSeed);
            SpecialSpellTimer.Elapsed += SpecialSpellEvent;
        }

        public void SetFightManager(FightManager fightManager)
        {
            this.FightManager = fightManager;
            TempCharacters.AddRange(fightManager.Characters);
            TempCharacters.Remove(this);
        }

        public async Task<Character> Start()
        {
            foreach (Character character in TempCharacters)
            {
                character.DeadCharacterEvent += DeleteDeadCharacter;
            }

            PoisonEventTimer(5000);

            while (this.CurrentLife > 0 && FightManager.Characters.Count > 1)
            {
                await DamageTakenDelayTask();
                await DefaultAttackTask();
            }

            foreach (Character character in TempCharacters)
            {
                character.DeadCharacterEvent -= DeleteDeadCharacter;
            }

            DeathEventArgs args = new DeathEventArgs();
            args.DeadCharacter = this;

            // Send my death event to all other characters (all listeners)
            this.DeadCharacterEvent?.Invoke(this, args);   // Invoke the delegate

            return this;
        }

        virtual public void SpecialSpell()
        {
        }

        virtual public void Attack()
        {
            Character target = Target();

            if (target != null)
            {
                Console.WriteLine("{0} Attaque", this.Name, target.Name);

                int attackMarge = AttackMarge(target);

                if (attackMarge > 0)
                {
                    int damageDeal = attackMarge * this.DamageRate / 100;
                    DealCommonDamage(target, damageDeal, 1);

                    target.DelayAttacks.Add(damageDeal);

                    Console.WriteLine("{0} PV restant : {1} PV", target.Name, target.CurrentLife);
                }
                else
                {
                    Console.WriteLine("{0} : Echec de l'attaque !", this.Name);
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
            Timer poisonEventTimer = new Timer();
            poisonEventTimer.Elapsed += PoisonEvent;
            poisonEventTimer.Interval = interval;
            poisonEventTimer.Enabled = true;
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
                Console.WriteLine("{0} : {1} PDV", this.Name, this.CurrentLife);
                Console.WriteLine("{0} : empoisonnement", this.Name);
                Console.WriteLine("{0} : -{1} PDV", this.Name, poisonDamage);

                this.CurrentLife -= poisonDamage;
            }
        }

        // Event handler
        public virtual void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            Console.WriteLine("{0} : {1} est mort", this.Name, e.DeadCharacter.Name);

            // Delete the dead target
            FightManager.Characters.Remove(e.DeadCharacter);
        }

        public static void DealCommonDamage(Character target, int damageDeal, double rate)
        {
            int damage = (int)(damageDeal * rate);
            Console.WriteLine("{0} : -{1} PDV", target.Name, damageDeal);

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
                    // On l'ajoute à la liste des cible valide
                    validTarget.Add(currentCharacter);
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
    }
}

