using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Scripts.Configs;
using Assets.Scripts.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Factories
{
    public class HeroFactory
    {
        private readonly LevelSettings _levelSettings;
        private readonly PlayerData _playerData;
        private readonly AbilityFactory _abilityFactory;

        public HeroFactory(LevelSettings levelSettings, PlayerData playerData, AbilityFactory abilityFactory)
        {
            _levelSettings = levelSettings.ThrowIfNull();
            _playerData = playerData.ThrowIfNull();
            _abilityFactory = abilityFactory.ThrowIfNull();
        }

        public Transform Create()
        {
            CharacterConfig config = _levelSettings.HeroConfig;
            Transform hero = Object.Instantiate(config.Prefab);

            hero.TryGetComponent(out HeroComponents heroComponents);
            _playerData.SetHeroTransform(heroComponents);

            heroComponents.CharacterMovement.Initialize(config.MoveSpeed, config.RotationSpeed);
            heroComponents.Health.Initialize(config.MaxHealth, config.InvincibilityDuration);
            heroComponents.LootCollector.Initialize(config.AttractionRadius, config.PullSpeed);
            heroComponents.AbilityContainer.Add(_abilityFactory.CreateSwordStrike(hero));

            hero.SetPositionAndRotation(_levelSettings.GameAreaCenter + Vector3.up, Quaternion.identity);

            Camera.main.TryGetComponent(out Follower follower).ThrowIfFalse();
            follower.Follow(hero);

            return hero.transform;
        }
    }
}
