using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts.CollectorsSystem
{
    public abstract class BaseCollectable<T> : MonoBehaviour, ICollectable<T>, IPoolable where T : BaseCollectable<T>
    {
        public event Action<T> Collected;

        public void Collect()
        {
            Collected?.Invoke((T)this);
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
