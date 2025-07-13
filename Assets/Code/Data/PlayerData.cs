using Assets.Code;
using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerData
    {
        public Wallet Wallet { get; private set; }
        public HeroExperience Level { get; private set; }
        public Transform HeroTransform { get; private set; }
        public AbilityType StartAbility { get; private set; }

        public PlayerData(Wallet wallet, HeroExperience level)
        {
            Wallet = wallet.ThrowIfNull();
            Level = level.ThrowIfNull();
            StartAbility = AbilityType.SwordStrike;
        }

        public void SetHeroTransform(HeroComponents heroComponents)
        {
            HeroTransform = heroComponents.ThrowIfNull().transform;
        }

        public void SetStartAbility(AbilityType startAbility)
        {
            StartAbility = startAbility.ThrowIfNull();
        }
    }
}
