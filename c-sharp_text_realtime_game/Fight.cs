using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace c_sharp_text_realtime_game
{
    public class CharacterType
    {
        public Type Type;
        public int NumberWin;

        public CharacterType(Type type, int numberWin)
        {
            this.Type = type;
            this.NumberWin = numberWin;
        }
    }

    public class Fight
    {
        public List<Character> Characters = new List<Character>();
        public Character Winner;
        public List<CharacterType> RankCharacterTypes = new List<CharacterType>();
        private ConsoleKey ConsoleKey;
        private CancellationTokenSource CancellationTokenSource;

        public Fight(List<Character> characters, ConsoleKey consoleKey, CancellationTokenSource cancellationTokenSource)
        {
            this.Characters = characters;
            this.ConsoleKey = consoleKey;
            this.CancellationTokenSource = cancellationTokenSource;

            foreach (Character character in Characters)
            {
                character.SetFight(this);

                // Mise en place classement meilleurs type de persos
                if (!RankCharacterTypes.Any(m => m.Type == character.GetType()))
                {
                    RankCharacterTypes.Add(new CharacterType(character.GetType(), 0));
                }
            }
        }


        public async Task<Fight> Start()
        {
            Task cancel = Task.Run(() => RaceCancellation());

            Task startFight = Task.Run(() => StartBattleRoyal(), CancellationTokenSource.Token);

            List<Task> tasks = new List<Task> { startFight, cancel };

            Task finished = await Task.WhenAny(tasks);

            if (finished == cancel)
            {
                Console.WriteLine("Fight Cancelled.");

                WriteJsonFile(Characters);

                return this;
            }
            else
            {
                return this;
            }
        }

        public async Task<Fight> StartBattleRoyal()
        {
            int score = 0;
            List<Task<Character>> attackTasks = new List<Task<Character>>();

            Console.WriteLine("Starting Battle Royal");

            foreach (Character character in Characters)
            {
                attackTasks.Add(character.Start());
            }

            while (attackTasks.Count > 1)
            {
                Task<Character> charaterDead = await Task.WhenAny(attackTasks);
                attackTasks.Remove(charaterDead);
                score++;

                // Calcul du score pour chaque type
                foreach (CharacterType characterType in RankCharacterTypes)
                {
                    if (characterType.Type == charaterDead.Result.GetType())
                    {
                        characterType.NumberWin += score;

                        Console.WriteLine("Type de personnage {0}", characterType.Type.Name);
                        Console.WriteLine("Avec un score : {0}", characterType.NumberWin);
                    }
                }
            }

            // Il y a un winner
            if (attackTasks.Count == 1)
            {
                score++;
                foreach (Task<Character> winner in attackTasks)
                {
                    this.Winner = winner.Result;
                }

                foreach (CharacterType characterType in RankCharacterTypes)
                {
                    if (characterType.Type == this.Winner.GetType())
                    {
                        characterType.NumberWin += score;

                        Console.WriteLine("Type de personnage {0}", characterType.Type.Name);
                        Console.WriteLine("Avec un score : {0}", characterType.NumberWin);
                    }
                }

                Console.WriteLine("Winner is : {0}", this.Winner.Name);
                Console.WriteLine("{0} PV restant : {1} PV", this.Winner.Name, this.Winner.CurrentLife);

                Console.WriteLine("Finish");

                return this;
            }

            Console.WriteLine("Finish");
            Console.WriteLine("No Winner");

            return this;
        }

        private async Task RaceCancellation()
        {
            Console.WriteLine("Press the " + ConsoleKey + " key to cancel the fight");
            while (Console.ReadKey(true).Key != ConsoleKey)
            {
                //Console.WriteLine("Press the any key to cancel...");
            }

            CancellationTokenSource.Cancel();
        }

        static void WriteJsonFile(List<Character> characters)
        {
            string data = JsonConvert.SerializeObject(characters, Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            string path = Path.Combine(Environment.CurrentDirectory, "save.json");
            File.WriteAllText(path, data);
        }

    }
}

