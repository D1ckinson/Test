using Assets.Characters;
using UnityEngine;

namespace Assets.Scripts.Spawners.Configs
{
    [CreateAssetMenu(fileName = "New Enemy Spawner Config", menuName = "Game/EnemySpawnerConfig")]
    internal class EnemySpawnerConfig : SpawnerConfig<Enemy> { }
}
