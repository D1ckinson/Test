﻿using Assets.Code.CharactersLogic.EnemyLogic;
using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.AbilitySystem.Abilities
{
    public class BlackHoleProjectile : MonoBehaviour
    {
        [SerializeField][Min(0.1f)] private float _baseDamage = 1f;
        [SerializeField][Min(0.1f)] private float _radiusForEnemy = 0.2f;
        [SerializeField][Min(0.1f)] private float _lifeTime = 5f;
        [SerializeField] private SphereCollider _effectZone;

        private readonly List<EnemyComponents> _enemies = new();
        private readonly Timer _timer = new();

        private LayerMask _damageLayer;
        private Pool<ParticleSystem> _effectPool;
        private float _damage;
        private float _pullForce;
        private float _radius;

        private void OnEnable()
        {
            _timer.Start(_lifeTime);
            _timer.Completed += Stop;

            void Stop()
            {
                _timer.Completed -= Stop;
                this.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _enemies.Clear();
            SetShape();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_damageLayer.Contains(other.gameObject.layer) && other.TryGetComponent(out EnemyComponents enemy))
            {
                _enemies.Add(enemy);
                SetShape();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out EnemyComponents enemy))
            {
                _enemies.Remove(enemy);
                SetShape();
            }
        }

        private void FixedUpdate()
        {
            foreach (EnemyComponents enemy in _enemies)
            {
                enemy.Health.TakeDamage(_baseDamage + _damage * _enemies.Count);

                Vector3 directionToCenter = (transform.position - enemy.transform.position).normalized;
                float pullForce = _pullForce * _enemies.Count;

                enemy.Rigidbody.AddForce(directionToCenter * pullForce);
            }
        }

        public BlackHoleProjectile Initialize(LayerMask damageLayer, float damage, float radius, float pullForce, Pool<ParticleSystem> effectPool)
        {
            _damageLayer = damageLayer.ThrowIfNull();
            _effectPool = effectPool.ThrowIfNull();

            SetStats(damage, radius, pullForce);

            return this;
        }

        public void SetStats(float damage, float radius, float pullForce)
        {
            _damage = damage.ThrowIfNegative();
            _pullForce = pullForce.ThrowIfNegative();
            _radius = radius.ThrowIfNegative();

            SetShape();
        }

        public void Activate(Vector3 position)
        {
            transform.position = position;

            this.SetActive(true);

            ParticleSystem effect = _effectPool.Get();
            effect.transform.position = position;
            effect.Play();
        }

        private void SetShape()
        {
            float radius = _radius + _radiusForEnemy * _enemies.Count;

            _effectZone.radius = radius;
            _effectPool?.ForEach(effect => effect.SetShapeRadius(radius));
        }
    }
}
