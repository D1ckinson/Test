using Assets.Scripts;
using Assets.Scripts.Configs;
using Assets.Scripts.Factories;
using Assets.Scripts.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Code.CharactersLogic
{
    public class DeathTriger : MonoBehaviour
    {
        private Health _health;
        private LootFactory _lootFactory;
        private CharacterConfig _characterConfig;

        public void Initialize(Health health, LootFactory lootFactory, CharacterConfig characterConfig)
        {
            _health = health.ThrowIfNull();
            _lootFactory = lootFactory.ThrowIfNull();
            _characterConfig = characterConfig.ThrowIfNull();
            _health.Died += SpawnLoot;
        }

        private void SpawnLoot()
        {
            foreach (LootConfig lootConfig in _characterConfig.Loot)
            {
                if (Random.Range(Constants.Zero, Constants.MaxChance) > lootConfig.DropChance)
                {
                    continue;
                }

                _lootFactory.Spawn(lootConfig.Prefab, transform.position, lootConfig.Count);
            }
        }
    }
}
