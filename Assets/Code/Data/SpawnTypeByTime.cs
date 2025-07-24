using System;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    public partial class LevelSettings
    {
        [Serializable]
        private struct SpawnTypeByTime
        {
            [field: SerializeField] public int Time { get; private set; }
            [field: SerializeField] public CharacterType Type { get; private set; }
        }
    }
}
