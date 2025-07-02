using UnityEngine;

namespace Assets.Scripts.Characters.EnemyConfigs
{
    [CreateAssetMenu(fileName = "New Enemy Item Drop Config", menuName = "Game/EnemyItemDropConfig")]
    internal class EnemyItemDropConfig : ScriptableObject
    {
        [field: SerializeField][field: Min(10)] public int ExperienceValue { get; private set; } = 10;
        [field: SerializeField][field: Min(5)] public int CoinDropChance { get; private set; } = 5;
        [field: SerializeField][field: Min(10)] public int CoinValue { get; private set; } = 10;
    }
}
