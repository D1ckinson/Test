using System;
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
        //и тд.
    }
}
