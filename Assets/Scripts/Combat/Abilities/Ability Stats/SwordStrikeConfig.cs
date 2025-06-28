using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Scripts.Combat.Abilities
{
    [CreateAssetMenu(fileName = "New Sword Strike Stats", menuName = "Game/SwordStrikeStats")]
    internal class SwordStrikeConfig : DamageSourceConfig
    {
        [field: Header("Basic Settings")]
        [field: SerializeField][field: Min(1)] public int Level { get; private set; } = 1;
        [field: SerializeField][field: Min(0.1f)] public float Cooldown { get; private set; } = 2f;

        [field: Header("Area Settings")]
        [field: SerializeField][field: Min(1)] public float Radius { get; private set; } = 4f;
        [field: SerializeField][field: Min(1)] public float Angle { get; private set; } = 50f;

        [field: Header("Slow Settings")]
        [field: SerializeField] public bool IsSlowEnable { get; private set; } = false;
        [SerializeField][Min(0.1f)] private float _slowDuration = 0.1f;
        [SerializeField][Min(0.1f)] private float _slowMultiplier = 0.1f;

        //private SpeedModifier _speedModifier;

        public float AngleCos { get; private set; }
        public float HalfAngle { get; private set; }

        private void Awake()
        {
            AngleCos = Mathf.Cos(Angle * Mathf.Deg2Rad);
            HalfAngle = Angle / Constants.Two;

            if (IsSlowEnable)
            {
                //_speedModifier = new(_slowMultiplier, _slowDuration);
            }
        }

        //public SpeedModifier GetSpeedModifier()
        //{
        //    return _speedModifier;
        //}
    }
}
