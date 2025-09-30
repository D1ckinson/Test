using Assets.Code.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Shop
{
    [CreateAssetMenu(menuName = "Game/UpgradeCost")]
    public class UpgradeCost : ScriptableObject
    {
        [field: SerializeField] private Cost[] _costArray;

        private Dictionary<AbilityType, Cost> _cost;

        public UpgradeCost Initialize()
        {
            _cost = _costArray.ToDictionary(cost => cost.AbilityType);

            return this;
        }

        public int GetCost(AbilityType abilityType, int level)
        {
            return _cost.GetValueOrThrow(abilityType).GetFor(level);
        }

        [Serializable]
        public struct Cost
        {
            [SerializeField] public AbilityType AbilityType;
            [field: SerializeField] int[] _cost;

            public readonly int GetFor(int level)
            {
                level--;
                level.ThrowIfZeroOrLess().ThrowIfMoreThan(_cost.LastIndex());

                return _cost[level];
            }
        }
    }
}
