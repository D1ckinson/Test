using Assets.Scripts;
using Assets.Scripts.Configs;
using Assets.Scripts.Factories;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Code.CharactersLogic
{
    public class DeathTriger : MonoBehaviour
    {
        private Health _health;
        private LootFactory _lootFactory;
        private LootConfig[] _loots;

        public void Initialize(Health health, LootFactory lootFactory, LootConfig[] loots)
        {
            _health = health.ThrowIfNull();
            _lootFactory = lootFactory.ThrowIfNull();
            _loots = loots.ThrowIfNullOrEmpty();
            _health.Died += SpawnLoot;
        }

        public void SetLoot(LootConfig[] loots)
        {
            _loots = loots.ThrowIfNullOrEmpty();
        }

        private void SpawnLoot()
        {
            foreach (LootConfig lootConfig in _loots)
            {
                if (Random.Range(Constants.Zero, Constants.Hundred) > lootConfig.DropChance)
                {
                    continue;
                }

                _lootFactory.Spawn(lootConfig.Prefab, transform.position, lootConfig.Count);
            }
        }
    }
}
