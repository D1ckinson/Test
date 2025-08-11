using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(menuName = "Game/AbilityConfig")]
    public class AbilityConfig : ScriptableObject
    {
        [field: Header("Base Settings")]
        [field: SerializeField] public Transform ProjectilePrefab { get; private set; }
        [field: SerializeField] public ParticleSystem Effect { get; private set; }
        [field: SerializeField] public LayerMask DamageLayer { get; private set; }
        [field: SerializeField] public AbilityType Type { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }

        [Header("Upgrades")]
        [SerializeField] private List<AbilityStats> _abilityStats;

        public int MaxLevel => _abilityStats.Count;

        public AbilityStats GetStats(int level)
        {
            level.ThrowIfZeroOrLess().ThrowIfMoreThan(MaxLevel);

            return _abilityStats[level - Constants.One];
        }
    }

    public class StaticData
    {

    }
}
