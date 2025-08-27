using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Code.Tools;
using Assets.Scripts.Configs;
using Assets.Scripts.Movement;
using Assets.Scripts.Tools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Factories
{
    public class HeroFactory
    {
        private readonly CharacterConfig _heroConfig;
        private readonly Wallet _wallet;
        private readonly HeroLevel _heroLevel;
        private readonly InputReader _inputReader;

        public HeroFactory(CharacterConfig heroConfig, Wallet wallet, HeroLevel heroLevel, InputReader inputReader)
        {
            _heroConfig = heroConfig.ThrowIfNull();
            _wallet = wallet.ThrowIfNull();
            _heroLevel = heroLevel.ThrowIfNull();
            _inputReader = inputReader.ThrowIfNull();
        }

        public HeroComponents Create(Vector3 position)
        {
            Transform hero = Object.Instantiate(_heroConfig.Prefab, position + Vector3.up, Quaternion.identity);// убрать "+ Vector3.up"
            HeroComponents heroComponents = hero.GetComponentOrThrow<HeroComponents>();

            heroComponents.CharacterMovement.Initialize(_heroConfig.MoveSpeed, _heroConfig.RotationSpeed, _inputReader);
            heroComponents.Health.Initialize(_heroConfig.MaxHealth, _heroConfig.InvincibilityDuration);
            heroComponents.LootCollector.Initialize(_heroConfig.AttractionRadius, _heroConfig.PullSpeed, _wallet, _heroLevel);

            Camera.main.GetComponentOrThrow<Follower>().Follow(hero);

            return heroComponents;
        }
    }
}
