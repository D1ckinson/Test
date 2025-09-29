﻿using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem.Abilities
{
    public class GhostSwords : Ability
    {
        private readonly WaitForSeconds _actionWait = new(0.1f);
        private readonly Transform _hero;
        private readonly Pool<GhostSword> _pool;
        private readonly List<GhostSword> _spawnedSwords = new();

        private float _damage;
        private int _projectilesCount;

        public GhostSwords(AbilityConfig config, Transform transform, int level = 1) : base(config, transform, level)
        {
            AbilityStats stats = config.ThrowIfNull().GetStats(level.ThrowIfZeroOrLess());
            _damage = stats.Damage.ThrowIfNegative();
            _projectilesCount = stats.ProjectilesCount.ThrowIfNegative();
            _hero = transform.ThrowIfNull();

            GhostSword CreateSword()
            {
                GhostSword sword = config.ProjectilePrefab.GetComponentOrThrow<GhostSword>().Instantiate();
                sword.Initialize(_damage, stats.IsPiercing, config.DamageLayer);

                return sword;
            }

            _pool = new(CreateSword);
        }

        protected override void Apply()
        {
            CoroutineService.StartCoroutine(SpawnSwords());
        }

        protected override void UpdateStats(float damage, float range, int projectilesCount, bool isPiercing)
        {
            _damage = damage.ThrowIfNegative();
            _projectilesCount = projectilesCount.ThrowIfNegative();
            _pool.ForEach(sword => sword.SetStats(damage, isPiercing));
        }

        private IEnumerator SpawnSwords()
        {

            for (int i = Constants.Zero; i < _projectilesCount; i++)
            {
                GhostSword sword = _pool.Get(false);
                sword.transform.SetParent(_hero);

                float angle = i * (Constants.FullCircleDegrees / _projectilesCount);
                Vector3 position = CalculateSwordPosition(angle);

                sword.transform.SetPositionAndRotation(position, Quaternion.Euler(Constants.Zero, angle, Constants.Zero));
                sword.SetActive(true);
                _spawnedSwords.Add(sword);

                yield return _actionWait;
            }

            CoroutineService.StartCoroutine(LaunchSwords());
        }

        private IEnumerator LaunchSwords()
        {
            for (int i = _spawnedSwords.LastIndex(); i >= Constants.Zero; i--)
            {
                _spawnedSwords[i].Launch();
                _spawnedSwords.RemoveAt(i);

                yield return _actionWait;
            }
        }

        private Vector3 CalculateSwordPosition(float angle)
        {
            Vector3 playerPosition = GetPosition();
            float radianAngle = angle * Mathf.Deg2Rad;

            float x = playerPosition.x + Mathf.Cos(radianAngle) * Constants.One;
            float z = playerPosition.z + Mathf.Sin(radianAngle) * Constants.One;

            return new Vector3(x, playerPosition.y, z);
        }
    }
}
