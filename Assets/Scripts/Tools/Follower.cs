using UnityEngine;

namespace Assets.Scripts.Tools
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;

        private Transform _transform;
        private Transform _target;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            if (_target == null)
            {
                return;
            }

            _transform.position = _target.position + _offset;
        }

        public void Follow(Transform target)
        {
            target.ThrowIfNull();
            _target = target;
        }
    }
}
