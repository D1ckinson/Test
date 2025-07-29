using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(SphereCollider))]
    public class LootCollector : MonoBehaviour
    {
        private const float CollectDistance = 1f;

        private readonly List<Loot> _toCollect = new();

        private float _pullSpeed;
        private SphereCollider _collectArea;

        private float _attractionRadius;

        private void Awake()
        {
            _collectArea = GetComponent<SphereCollider>().ThrowIfNull();
            _collectArea.isTrigger = true;
        }

        private void Update()
        {
            for (int i = _toCollect.GetLastIndex(); i >= Constants.Zero; i--)
            {
                Loot loot = _toCollect[i];

                Vector3 distanceVector = transform.position - loot.transform.position;
                float sqrDistance = distanceVector.sqrMagnitude;

                Transform lootTransform = loot.transform;
                lootTransform.position += _pullSpeed * Time.deltaTime * distanceVector;

                if (sqrDistance > CollectDistance)
                {
                    continue;
                }

                loot.Collect();
                _toCollect.Remove(loot);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Loot loot) == false || _toCollect.Contains(loot))
            {
                return;
            }

            _toCollect.Add(loot);
        }

        public void Initialize(float attractionRadius, float pullSpeed)
        {
            _attractionRadius = attractionRadius.ThrowIfNegative();
            _collectArea.radius = _attractionRadius;

            _pullSpeed = pullSpeed.ThrowIfZeroOrLess();
        }

        public void AddAttractionRadius(int value)
        {
            _collectArea.radius = _attractionRadius + value.ThrowIfNegative();
        }
    }
}
