using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem
{
    public readonly struct UpgradeOption
    {
        public readonly AbilityType Type;
        public readonly int NextLevel;
        public readonly List<string> Stats;
        public readonly Sprite Icon;
        public readonly string Name;

        public UpgradeOption(AbilityType type, int nextLevel, List<string> statsDescription, Sprite icon, string name)
        {
            Type = type.ThrowIfNull();
            NextLevel = nextLevel.ThrowIfZeroOrLess();
            Stats = statsDescription.ThrowIfNullOrEmpty();
            Icon = icon.ThrowIfNull();
            Name = name.ThrowIfNullOrEmpty();
        }
    }
}