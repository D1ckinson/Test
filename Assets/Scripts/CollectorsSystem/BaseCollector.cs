using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.CollectorsSystem
{
    public class BaseCollector<T> : MonoBehaviour where T : MonoBehaviour, ICollectable<T>
    {
        private const float CollectDistance = 1f;

        [SerializeField] private SphereCollider _collectArea;
        [SerializeField][Min(1f)] private float _attractionRadius = 10f;
        [SerializeField][Min(1f)] private float _pullSpeed = 5f;

        private readonly List<T> _toCollect = new();
        private Transform _transform;

        public event Action<T> Collect;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _collectArea.ThrowIfNull();

            if (_collectArea == null)
            {
                return;
            }

            if (_collectArea.isTrigger == false)
            {
                _collectArea.isTrigger = true;
            }

            if (_collectArea.radius != _attractionRadius)
            {
                _collectArea.radius = _attractionRadius;
            }
        }

        private void OnDrawGizmos()
        {
            CustomGizmos.DrawCircle(transform.position, _attractionRadius, Color.red);
        }
#endif

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            for (int i = _toCollect.Count - Constants.One; i >= Constants.Zero; i--)
            {
                T collectable = _toCollect[i];

                Vector3 distanceVector = collectable.transform.position - _transform.position;
                float sqrDistance = distanceVector.sqrMagnitude;

                if (sqrDistance <= CollectDistance)
                {
                    _toCollect.Remove(collectable);

                    collectable.Collect();
                    Collect?.Invoke(collectable);

                    continue;
                }

                Transform collectableTransform = collectable.transform;
                collectableTransform.position = Vector3.MoveTowards(collectableTransform.position, _transform.position, _pullSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out T collectable) == false)
            {
                return;
            }

            if (_toCollect.Contains(collectable))
            {
                return;
            }

            _toCollect.Add(collectable);
        }
    }
}
