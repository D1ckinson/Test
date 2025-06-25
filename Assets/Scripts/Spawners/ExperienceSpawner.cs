using Assets.Scripts.ExperienceSystem;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    internal class ExperienceSpawner : MonoBehaviour
    {
        [SerializeField] private List<Experience> _experiencePrefabs;
        [SerializeField][Min(0.1f)] private float _maxSpawnOffset = 1f;

        private Dictionary<int, List<Experience>> _spawnLists;
        private Dictionary<int, Pool<Experience>> _experiencePools;
        private int _minValue;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _experiencePrefabs.ThrowIfNull();
            _experiencePrefabs?.ForEach(value => value.ThrowIfNull());
        }
#endif

        private void Awake()
        {
            _spawnLists = new();
            _minValue = _experiencePrefabs.Min(experience => experience.Value);
            _experiencePrefabs = _experiencePrefabs.OrderByDescending(experience => experience.Value).ToList();
            _experiencePools = new();

            foreach (var prefab in _experiencePrefabs)
            {
                _experiencePools[prefab.Value] = new Pool<Experience>(() => CreateFunc(prefab));
            }
        }

        public void Spawn(Vector3 position, int value)
        {
            value.ThrowIfLessThan(_minValue);

            if (_spawnLists.TryGetValue(value, out List<Experience> spawnList) == false)
            {
                spawnList = CreateSpawnList(value);
                _spawnLists.Add(value, spawnList);
            }

            foreach (Experience experience in spawnList)
            {
                Vector3 spawnPosition = position + GenerateOffset();

                Experience spawnedExperience = _experiencePools[experience.Value].Get();
                spawnedExperience.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            }
        }

        private Experience CreateFunc(Experience experience)
        {
            Experience spawnedExperience = Instantiate(experience);
            spawnedExperience.Collected += OnExperienceCollected;

            return spawnedExperience;
        }

        private void OnExperienceCollected(Experience experience)
        {
            experience.ThrowIfNull();
            _experiencePools[experience.Value].Return(experience);
        }

        private List<Experience> CreateSpawnList(int value)
        {
            List<Experience> result = new();
            int remainingValue = value;

            foreach (Experience experience in _experiencePrefabs)
            {
                while (remainingValue >= experience.Value)
                {
                    result.Add(experience);
                    remainingValue -= experience.Value;
                }
            }

#if UNITY_EDITOR
            if (remainingValue > Constants.Zero)
            {
                string message = $"Не удалось разбить {value} на имеющиеся значения! Остаток: {remainingValue}";
                Debug.LogWarning(message);
            }
#endif

            return result;
        }

        private Vector3 GenerateOffset()
        {
            Vector3 offset = new()
            {
                x = Random.Range(Constants.Zero, _maxSpawnOffset),
                z = Random.Range(Constants.Zero, _maxSpawnOffset)
            };

            return offset;
        }
    }
}
