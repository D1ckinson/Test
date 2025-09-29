using Assets.Code;
using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    [Serializable]
    public class PlayerData
    {
        public Wallet Wallet;
        public AbilityType StartAbility;
        public Dictionary<AbilityType, int> AbilityUnlockLevel;
        public float ScoreRecord;

        public PlayerData()
        {
            Wallet = new();
            StartAbility = AbilityType.GhostSwords;

            AbilityUnlockLevel = new()
            {
                [AbilityType.SwordStrike] = 5,
                [AbilityType.GhostSwords] = 5,
            };
        }
    }
}
