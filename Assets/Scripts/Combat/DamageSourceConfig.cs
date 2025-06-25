using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Combat
{
    [CreateAssetMenu(fileName = "NewDamageSourceConfig", menuName = "Game/DamageSourceConfig")]
    internal class DamageSourceConfig : ScriptableObject
    {
        [field: SerializeField][field: Min(1f)] internal float Value { get; private set; } = 1f;
        [SerializeField] private List<EntityType> _entityTypes;

        internal bool CanDamage(EntityType entityType)
        {
            return _entityTypes.Contains(entityType);
        }
    }
}