using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem
{
    public readonly struct UpgradeOption
    {
        public readonly AbilityType Type;
        public readonly int NextLevel;
        public readonly List<string> Text;
        public readonly Sprite Icon;

        public UpgradeOption(AbilityType type, int nextLevel, List<string> statsDescription, Sprite icon)
        {
            Type = type.ThrowIfNull();
            NextLevel = nextLevel.ThrowIfZeroOrLess();
            Text = statsDescription.ThrowIfCollectionNullOrEmpty();
            Icon = icon.ThrowIfNull();
        }
    }
}