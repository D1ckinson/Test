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
        private readonly Wallet _wallet;
        private readonly HeroLevel _heroLevel;

        public LootFactory(List<Loot> loots, Wallet wallet, HeroLevel heroLevel)
        {
            loots.ThrowIfNullOrEmpty();
            _wallet = wallet.ThrowIfNull();
            _heroLevel = heroLevel.ThrowIfNull();

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
                LootType.LowExperience => _heroLevel,
                LootType.MediumExperience => _heroLevel,
                LootType.HighExperience => _heroLevel,
                LootType.Coin => _wallet,
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
