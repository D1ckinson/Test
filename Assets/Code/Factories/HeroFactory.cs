using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Scripts.Configs;
using Assets.Scripts.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Factories
{
    public class HeroFactory
    {
        private readonly CharacterConfig _heroConfig;

        public HeroFactory(CharacterConfig heroConfig)
        {
            _heroConfig = heroConfig.ThrowIfNull();
        }

        public HeroComponents Create(Vector3 position)
        {
            Transform hero = Object.Instantiate(_heroConfig.Prefab, position + Vector3.up, Quaternion.identity);// убрать "+ Vector3.up"
            hero.TryGetComponent(out HeroComponents heroComponents);

            heroComponents.CharacterMovement.Initialize(_heroConfig.MoveSpeed, _heroConfig.RotationSpeed);
            heroComponents.Health.Initialize(_heroConfig.MaxHealth, _heroConfig.InvincibilityDuration);
            heroComponents.LootCollector.Initialize(_heroConfig.AttractionRadius, _heroConfig.PullSpeed);

            Camera.main.TryGetComponent(out Follower follower).ThrowIfFalse();
            follower.Follow(hero);

            return heroComponents;
        }
    }
}
