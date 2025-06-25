using Assets.Scripts.Characters;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    public class HeroSpawner : MonoBehaviour
    {
        [SerializeField] private Hero _prefab;
        [SerializeField] private Follower _camera;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _prefab.ThrowIfNull();
            _camera.ThrowIfNull();
        }
#endif

        public Transform Spawn()
        {
            Hero hero = Instantiate(_prefab, transform.position, Quaternion.identity);
            hero.Initialize();

            _camera.Follow(hero.transform);

            return hero.transform;
        }
    }
}
