using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            while (CurrentLife > 0 && FightManager.Characters.Count > 1)
            {
                await DamageTakenDelayTask();
                await Task.WhenAny(DelayDefaultAttackTask(), DelaySpecialSpellTask());
            }

            // I'm dead
            FightManager.Characters.Remove(this);

            foreach (Character character in TempCharacters)
            {
                character.DeadCharacterEvent -= DeleteDeadCharacter;
            }

            DeathEventArgs args = new DeathEventArgs();
            args.DeadCharacter = this;

            // Send my death event to all other characters (all listeners)
            DeadCharacterEvent?.Invoke(this, args);   // Invoke the delegate

            return this;
        }

        virtual public void Passive()
        {

        }

        virtual public Task SpecialSpell()
        {
            return Task.CompletedTask;
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
                    int damageDeal = attackMarge * DamageRate / 100;
                    DealCommonDamage(target, damageDeal);

                    target.DelayAttacks.Add(damageDeal);

                    Console.WriteLine("{0} PV restant : {1} PV", target.Name, target.CurrentLife);
                }
                else
                {
                    Console.WriteLine("{0} : Echec de l'attaque !", this.Name);
                }
            }
        }

        public static void DealCommonDamage(Character target, int damageDeal)
        {
            Console.WriteLine("{0} : -{1} PDV", target.Name, damageDeal);
            target.CurrentLife -= damageDeal;
        }

        public Task DelayDefaultAttackTask()
        {
            Task<Task> delayAttack = DelayAttack(AttackSpeed);

            return Task.Run(async () =>
            {
                if (CurrentLife > 0)
                {
                    Attack();
                }

                delayAttack.Start(TaskScheduler.Default);
                await delayAttack.Unwrap();
            });
        }
        public Task DelaySpecialSpellTask()
        {
            Task<Task> delaySpecialSpell = DelayAttack(PowerSpeed);

            return Task.Run(async () =>
            {
                if (CurrentLife > 0)
                {
                    await SpecialSpell();
                }

                delaySpecialSpell.Start(TaskScheduler.Default);
                await delaySpecialSpell.Unwrap();
            });
        }
        public Task DamageTakenDelayTask()
        {
            Task<Task> damageTakenDelayAttack = DamageTakenDelayAttack();

            return Task.Run(async () =>
            {
                damageTakenDelayAttack.Start(TaskScheduler.Default);
                await damageTakenDelayAttack.Unwrap();
            });
        }

        public Task<Task> DelayAttack(double speed)
        {
            return new Task<Task>(async () =>
            {
                int delayAttack = (int)((1000 / speed) - RollDice());
                await Task.Delay(delayAttack);
            });
        }

        public virtual Task<Task> DamageTakenDelayAttack()
        {
            return new Task<Task>(async () =>
            {
                int delayAttack = 0;

                DelayAttacks.ForEach(delay =>
                {
                    delayAttack += delay;
                });
                await Task.Delay(delayAttack);
            });
        }

        // Event handler
        public virtual void DeleteDeadCharacter(object sender, DeathEventArgs e)
        {
            Console.WriteLine("{0} : {1} est mort", this.Name, e.DeadCharacter.Name);
            TempCharacters.Remove(e.DeadCharacter);
        }






        // Selectionner une cible valide
        public virtual Character Target()
        {
            // On cree une liste dans laquelle on stockera les cibles valides
            List<Character> validTarget = new List<Character>();

            for (int i = 0; i < FightManager.Characters.Count; i++)
            {
                Character currentCharacter = FightManager.Characters[i];
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
                Character target = validTarget[Random.Next(0, validTarget.Count)];
                return target;
            }
            return null;
        }

        protected int AttackMarge(Character target)
        {
            return AttackRoll() - target.DefenseRoll();
        }

        protected virtual int AttackRoll()
        {
            return AttackRate + RollDice();
        }

        protected virtual int DefenseRoll()
        {
            return DefenseRate + RollDice();
        }

        private int RollDice()
        {
            return Random.Next(1, 101);
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

