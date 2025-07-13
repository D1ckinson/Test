using Assets.Code.CharactersLogic.HeroLogic;
using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class CollisionDamage : MonoBehaviour
    {
        private float _damage;
        private LayerMask _damageLayer;

        private void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            HandleCollision(collision);
        }

        public void Initialize(float damage, LayerMask damageLayer)
        {
            _damage = damage.ThrowIfNegative();
            _damageLayer = damageLayer.ThrowIfNull();
        }

        private void HandleCollision(Collision collision)
        {
            GameObject gameObject = collision.ThrowIfNull().gameObject;

            if (gameObject.TryGetComponent(out HeroComponents heroComponents))
            {
                heroComponents.Health.TakeDamage(_damage);

                return;
            }

            if (gameObject.layer == _damageLayer && gameObject.TryGetComponent(out Health health))
            {
                health.TakeDamage(_damage);
            }
        }
    }
}
