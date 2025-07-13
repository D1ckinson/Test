﻿using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(menuName = "Game/CharacterConfig")]
    public class CharacterConfig : ScriptableObject
    {
        [field: Header("Character Settings")]
        [field: SerializeField] public CharacterType Type { get; private set; }
        [field: SerializeField] public Transform Prefab { get; private set; }

        [field: Header("Health Settings")]
        [field: SerializeField][field: Min(1f)] public float MaxHealth { get; private set; } = 100;
        [field: SerializeField][field: Min(0.1f)] public float InvincibilityDuration { get; private set; } = 0.1f;

        [field: Header("Movement Settings")]
        [field: SerializeField][field: Min(1f)] public float MoveSpeed { get; private set; } = 10;
        [field: SerializeField][field: Min(1f)] public float RotationSpeed { get; private set; } = 200;

        [field: Header("Contact Damage Settings")]
        [field: SerializeField][field: Min(10)] public int Damage { get; private set; } = 10;
        [field: SerializeField] public LayerMask DamageLayer { get; private set; }

        [field: Header("loot Collection Settings")]
        [field: SerializeField][field: Min(1f)] public float AttractionRadius { get; private set; } = 10f;
        [field: SerializeField][field: Min(1f)] public float PullSpeed { get; private set; } = 5f;

        [field: Header("Loot Droop Settings")]
        [field: SerializeField] public LootConfig[] Loot { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            DamageLayer.ThrowIfNull();
        }
#endif
    }
}
