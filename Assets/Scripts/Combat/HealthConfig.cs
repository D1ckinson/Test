namespace Assets.Scripts.Combat
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "New Health Config", menuName = "Game/HealthConfig")]
    public class HealthConfig : ScriptableObject
    {
        [field: SerializeField][field: Min(1f)] public float MaxValue { get; private set; } = 100;
        [field: SerializeField][field: Min(0.1f)] public float InvincibilityDuration { get; private set; } = 0.1f;
    }
}