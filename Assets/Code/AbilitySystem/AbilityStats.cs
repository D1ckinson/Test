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
        [field: SerializeField] public float ProjectilesCount { get; private set; }

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

        public List<string> GetStatsDifference(AbilityStats other)
        {
            List<string> result = new();

            AddIfPositive(result, "Перезарядка", Cooldown - other.Cooldown);
            AddIfPositive(result, "Урон", Damage - other.Damage);
            AddIfPositive(result, "Дальность", Range - other.Range);
            AddIfPositive(result, "Количество снарядов", ProjectilesCount - other.ProjectilesCount);

            return result;
        }

        private void AddIfPositive(List<string> list, string statName, float difference)
        {
            if (difference > 0)
            {
                string formattedValue = difference % 1 == 0
                    ? difference.ToString("+0;-0") // Целые числа без .0
                    : difference.ToString("+0.0;-0.0"); // Дробные с одним знаком

                list.Add($"{statName} {formattedValue}");
            }
        }
    }
}
