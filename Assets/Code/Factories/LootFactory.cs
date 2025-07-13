using Assets.Code.Data.Interfaces;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Factories
{
    public class LootFactory
    {
        private readonly Dictionary<LootType, Pool<Loot>> _pools;
        private readonly PlayerData _playerData;

        public LootFactory(List<Loot> loots, PlayerData playerData)
        {
            loots.ThrowIfCollectionNull();
            _playerData = playerData.ThrowIfNull();

            _pools = new();
            loots.ForEach(loot => _pools.Add(loot.LootType, new(() => Create(loot))));
        }

        public void Spawn(Loot loot, Vector3 position, int count = Constants.One)
        {
            loot.ThrowIfNull();
            count.ThrowIfZeroOrLess();

            for (int i = Constants.Zero; i < count; i++)
            {
                Loot createdLoot = _pools[loot.LootType].Get();
                createdLoot.transform.SetPositionAndRotation(position + GenerateOffset(), GenerateRotation());
            }
        }

        private Loot Create(Loot loot)
        {
            Loot createdLoot = Object.Instantiate(loot);

            IValueContainer valueContainer = loot.LootType switch
            {
                LootType.LowExperience => _playerData.Level,
                LootType.MediumExperience => _playerData.Level,
                LootType.HighExperience => _playerData.Level,
                LootType.Coin => _playerData.Wallet,
                _ => throw new NotImplementedException(),
            };

            createdLoot.Initialize(valueContainer);

            return createdLoot;
        }

        private Vector3 GenerateOffset()
        {
            Vector3 offset = new()
            {
                x = Random.Range(Constants.Zero, Constants.One),
                z = Random.Range(Constants.Zero, Constants.One)
            };

            return offset;
        }

        private Quaternion GenerateRotation()
        {
            Quaternion rotation = new()
            {
                y = Random.Range(Constants.Zero, Constants.FullCircleDegrees),
            };

            return rotation;
        }
    }
}
