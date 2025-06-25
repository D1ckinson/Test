using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    internal class SpawnerConfig<T> : ScriptableObject where T : MonoBehaviour
    {
        [field: SerializeField] internal T Prefab { get; private set; }
        [field: SerializeField][field: Min(1)] internal float Radius { get; private set; } = 1f;
        [field: SerializeField][field: Min(0.1f)] internal float SpawnDelay { get; private set; } = 0.6f;
        [field: SerializeField][field: Min(1)] internal int MaxCount { get; private set; } = 50;

#if UNITY_EDITOR
        private void OnValidate()
        {
            Prefab.ThrowIfNull();
        }
#endif
    }
}
