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
            TryDamage(collision.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            collision.ThrowIfNull(); 
            TryDamage(collision.gameObject);
        }
    }
}
