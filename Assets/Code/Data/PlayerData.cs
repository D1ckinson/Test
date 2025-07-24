using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerData
    {
        public Wallet Wallet { get; private set; }
        public HeroExperience HeroExperience { get; private set; }
        public HeroComponents HeroComponents { get; private set; }
        public AbilityType StartAbility { get; private set; }

        public PlayerData(Wallet wallet, HeroExperience level)
        {
            Wallet = wallet.ThrowIfNull();
            HeroExperience = level.ThrowIfNull();
            StartAbility = AbilityType.SwordStrike;
        }

        public void SetHeroTransform(HeroComponents heroComponents)
        {
            HeroComponents = heroComponents.ThrowIfNull();
        }

        public void SetStartAbility(AbilityType startAbility)
        {
            StartAbility = startAbility.ThrowIfNull();
        }
    }
}
