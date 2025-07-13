using Assets.Code;
using Assets.Scripts.Configs;
using Assets.Scripts.Factories;
using Assets.Scripts.Tools;

namespace Assets.Scripts.State_Machine
{
    public class FactoryService
    {
        public EnemyFactory EnemyFactory { get; private set; }
        public HeroFactory HeroFactory { get; private set; }
        public LootFactory LootFactory { get; private set; }
        public AbilityFactory AbilityFactory { get; private set; }

        public FactoryService(LevelSettings levelSettings, PlayerData playerData)
        {
            LootFactory = new(levelSettings.Loots, playerData);
            EnemyFactory = new(levelSettings, playerData, LootFactory);
            AbilityFactory = new(levelSettings.AbilitiesConfigs);
            HeroFactory = new(levelSettings.ThrowIfNull(), playerData.ThrowIfNull(), AbilityFactory);
        }
    }
}
