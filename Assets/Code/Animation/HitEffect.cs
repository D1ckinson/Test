using Assets.Code.Animation.Types;
using Assets.Code.Tools;
using UnityEngine;

namespace Assets.Scripts
{
    public class HitEffect : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private readonly int _hash = Animator.StringToHash(AdditionalAnimations.HitEffect.ToString());

        private void Awake()
        {
            _animator.ThrowIfAnimationMissing(_hash);
        }

        public void Play()
        {
            _animator.PlayAsNew(_hash);
        }
    }
}
