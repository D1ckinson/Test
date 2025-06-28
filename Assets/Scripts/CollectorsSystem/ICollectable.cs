using System;

namespace Assets.Scripts.CollectorsSystem
{
    public interface ICollectable<T>
    {
        public event Action<T> Collected;

        public void Collect();
    }
}
