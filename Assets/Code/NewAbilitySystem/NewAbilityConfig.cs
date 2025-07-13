using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    [CreateAssetMenu(menuName = "Game/NewAbilityConfig")]
    public class NewAbilityConfig : ScriptableObject
    {
        [field: Header("Base Settings")]
        [field: SerializeField] public Transform ProjectilePrefab { get; private set; }
        [field: SerializeField] public ParticleSystem Effect { get; private set; }
        [field: SerializeField] public LayerMask DamageLayer { get; private set; }
        [field: SerializeField] public AbilityType Type { get; private set; }

        [Header("Upgrades")]
        [SerializeField] private List<AbilityStats> _abilityStats;

        public AbilityStats GetStats(int level)
        {
            return _abilityStats[level.ThrowIfNegative().ThrowIfMoreThan(_abilityStats.Count - Constants.One)];
        }
    }
}
