using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.ExperienceSystem
{
    internal class ExperienceCollector : MonoBehaviour
    {
        private const float CollectDistance = 1f;

        [SerializeField] private SphereCollider _collectArea;
        [SerializeField][Min(1f)] private float _attractionRadius = 10f;
        [SerializeField][Min(1f)] private float _pullSpeed = 5f;

        private readonly List<Experience> _experiences = new();
        private Transform _transform;

        internal event Action<int> Collect;

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            CustomGizmos.DrawCircle(transform.position,_attractionRadius,Color.red);
        }
#endif

        private void Awake()
        {
            _transform = transform;

            _collectArea.isTrigger = true;
            _collectArea.radius = _attractionRadius;
        }

        private void Update()
        {
            for (int i = _experiences.Count - Constants.One; i >= Constants.Zero; i--)
            {
                Experience experience = _experiences[i];

                Vector3 distanceVector = experience.transform.position - _transform.position;
                float sqrDistance = distanceVector.sqrMagnitude;

                if (sqrDistance <= CollectDistance)
                {
                    _experiences.Remove(experience);

                    int value = experience.Collect();
                    Collect?.Invoke(value);

                    continue;
                }

                Transform experienceTransform = experience.transform;
                experienceTransform.position = Vector3.MoveTowards(experienceTransform.position, _transform.position, _pullSpeed * Time.deltaTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Experience experience) == false)
            {
                return;
            }

            if (_experiences.Contains(experience))
            {
                return;
            }

            _experiences.Add(experience);
        }
    }
}
