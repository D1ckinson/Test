using UnityEngine;
using Assets.Scripts.Tools;
using System;

namespace Assets.Scripts.ExperienceSystem
{
    public class Experience : MonoBehaviour, IPollable
    {
        [field: SerializeField][field: Min(1)] public int Value { get; private set; } = 10;

        public event Action<Experience> Collected;

        internal int Collect()
        {
            Collected?.Invoke(this);

            return Value;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
