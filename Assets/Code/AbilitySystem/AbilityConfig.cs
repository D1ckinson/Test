using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(menuName = "Game/AbilityConfig")]
    public class AbilityConfig : ScriptableObject
    {
        [field: Header("Damage Settings")]
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public LayerMask DamageLayer { get; private set; }

        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
    }
}
