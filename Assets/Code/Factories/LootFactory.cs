using Assets.Code.Loot;
using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Factories
{
    public class LootFactory
    {
        private readonly Dictionary<LootType, Pool<Loot>> _pools;

        public LootFactory(Loot[] loots)
        {
            loots.ThrowIfNullOrEmpty();

            _pools = new();
            loots.ForEach(loot => _pools.Add(loot.Type, new(() => Create(loot))));
        }

        public void Spawn(Loot loot, Vector3 position, int count = Constants.One)
        {
            loot.ThrowIfNull();
            count.ThrowIfZeroOrLess();

            for (int i = Constants.Zero; i < count; i++)
            {
                Loot createdLoot = _pools[loot.Type].Get();
                createdLoot.transform.SetPositionAndRotation(position + GenerateOffset(), GenerateRotation());
            }
        }

        public void DisableAll()
        {
            _pools.ForEachValues(pool => pool.DisableAll());
        }

        private Loot Create(Loot loot)
        {
            return Object.Instantiate(loot);
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
