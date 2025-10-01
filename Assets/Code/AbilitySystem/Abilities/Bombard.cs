using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.AbilitySystem.Abilities
{
    public class Bombard : Ability
    {
        private const float MaxThrowDistance = 10f;

        private readonly WaitForSeconds _delay = new(0.1f);
        private readonly Pool<Bomb> _pool;

        private float _damage;
        private float _explosionRadius;
        private float _projectilesCount;

        public Bombard(AbilityConfig config, Transform transform, Dictionary<AbilityType, int> abilityUnlockLevel, int level = 1) : base(config, transform, abilityUnlockLevel, level)
        {
            AbilityStats stats = config.ThrowIfNull().GetStats(level);

            _damage = stats.Damage;
            _projectilesCount = stats.ProjectilesCount;
            _explosionRadius = stats.Range;

            _pool = new(CreateBomb);

            Bomb CreateBomb()
            {
                Bomb bomb = config.ProjectilePrefab.GetComponentOrThrow<Bomb>().Instantiate();
                bomb.Initialize(_damage, _explosionRadius, config.DamageLayer);

                return bomb;
            }
        }

        protected override void Apply()
        {
            CoroutineService.StartCoroutine(LaunchBombs(), this);
        }

        private IEnumerator LaunchBombs()
        {
            for (int i = Constants.Zero; i < _projectilesCount; i++)
            {
                Bomb bomb = _pool.Get();
                bomb.Fly(Position, GenerateRandomPoint());

                yield return _delay;
            }
        }

        public override void Dispose()
        {
            CoroutineService.StopAllCoroutines(this);
            _pool.ForEach(bomb => bomb.DestroyGameObject());
        }

        protected override void UpdateStats(float damage, float range, int projectilesCount, bool isPiercing, int healthPercent)
        {
            _damage = damage.ThrowIfNegative();
            _projectilesCount = projectilesCount.ThrowIfNegative();
            _explosionRadius = range.ThrowIfNegative();

            _pool.ForEach(bomb => bomb.SetStats(_damage, _explosionRadius));
        }

        private Vector3 GenerateRandomPoint()
        {
            float randomAngle = Random.Range(Constants.Zero, Constants.FullCircleDegrees) * Mathf.Deg2Rad;

            Vector3 direction = new(Mathf.Cos(randomAngle), Constants.Zero, Mathf.Sin(randomAngle));
            Vector3 distance = direction * Random.Range(Constants.One, MaxThrowDistance);
            Vector3 point = Position + distance;

            return point;
        }
    }
}
