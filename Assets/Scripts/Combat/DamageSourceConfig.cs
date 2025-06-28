using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    [CreateAssetMenu(fileName = "NewDamageSourceConfig", menuName = "Game/DamageSourceConfig")]
    public class DamageSourceConfig : ScriptableObject
    {
        [field: SerializeField][field: Min(1f)] public float Damage { get; private set; } = 1f;
        [SerializeField] private List<EntityType> _entityTypes;

        public bool CanDamage(EntityType entityType)
        {
            return _entityTypes.Contains(entityType);
        }
    }
}