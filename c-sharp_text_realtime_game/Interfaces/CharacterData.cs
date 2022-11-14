using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace c_sharp_text_realtime_game.Interfaces
{
    public class CharacterData
    {
        [JsonProperty] public string Name;
        [JsonProperty] public int AttackRate;
        [JsonProperty] public int DefenseRate;
        [JsonProperty] public double AttackSpeed;
        [JsonProperty] public int DamageRate;
        [JsonProperty] public int MaximumLife;
        [JsonProperty] public int CurrentLife;
        [JsonProperty] public double PowerSpeed;
        [JsonProperty] public ConsoleColor Color { get; set; }
        [JsonProperty] public String Type { get; set; }

        [JsonIgnore] public Random Random;
        [JsonIgnore] public int RandomSeed;
        [JsonIgnore] public Fight Fight;
        [JsonProperty] public List<int> DelayAttacks = new List<int>();
        [JsonProperty] public List<int> PoisonDamages = new List<int>();
        [JsonProperty] public bool IsSpecialSpellAvailable = true;


        public CharacterData()
        {
        }
    }
}

