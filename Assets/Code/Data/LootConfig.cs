using System;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [Serializable]
    public class LootConfig
    {
        [field: SerializeField] public Loot Prefab { get; private set; }
        [field: SerializeField][field: Min(1)] public int Count { get; private set; } = 1;
        [field: SerializeField][field: Min(1f)] public float DropChance { get; private set; } = 100;
    }
}
