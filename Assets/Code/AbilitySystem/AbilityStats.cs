﻿using Assets.Code.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [Serializable]
    public class AbilityStats
    {
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float Range { get; private set; }
        [field: SerializeField] public int ProjectilesCount { get; private set; }
        [field: SerializeField] public bool IsPiercing { get; private set; }

        public static AbilityStats operator -(AbilityStats a, AbilityStats b)
        {
            return new AbilityStats
            {
                Cooldown = a.Cooldown - b.Cooldown,
                Damage = a.Damage - b.Damage,
                Range = a.Range - b.Range,
                ProjectilesCount = a.ProjectilesCount - b.ProjectilesCount
            };
        }

        public List<string> GetStatsDescription()
        {
            List<string> description = new();

            AddIfNotZero(description, "Перезарядка", Cooldown);
            AddIfNotZero(description, "Урон", Damage);
            AddIfNotZero(description, "Дальность", Range);
            AddIfNotZero(description, "Количество снарядов", ProjectilesCount);

            return description;
        }

        private void AddIfNotZero(List<string> list, string statName, float value)
        {
            if (value == Constants.Zero)
            {
                return;
            }

            string formattedValue = value % Constants.One == Constants.Zero
                ? value.ToString("+0;-0")
                : value.ToString("+0.0;-0.0");

            list.Add($"{statName} {formattedValue}");
        }
    }
}
