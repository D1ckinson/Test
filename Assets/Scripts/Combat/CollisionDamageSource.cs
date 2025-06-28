using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    [RequireComponent(typeof(Collider))]
    public class CollisionDamageSource : DamageSourceBase<DamageSourceConfig>
    {
        private void OnCollisionEnter(Collision collision)
        {
            collision.ThrowIfNull();
            HandleCollider(collision.collider);
        }

        private void OnCollisionStay(Collision collision)
        {
            collision.ThrowIfNull();
            HandleCollider(collision.collider);
        }

        private void HandleCollider(Collider collision)
        {
            TryDamage(collision.gameObject);
        }
    }
}
