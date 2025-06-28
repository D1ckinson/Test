using System;
using System.Collections.Generic;

namespace Assets.Scripts.Tools
{
    public class Pool<T> where T : IPoolable
    {
        private readonly Queue<T> _items = new();
        private readonly Func<T> _createFunc;

        public Pool(Func<T> createFunc)
        {
            createFunc.ThrowIfNull();
            _createFunc = createFunc;
        }

        public int ReleaseCount { get; private set; }

        public T Get()
        {
            T item = _items.Count == Constants.Zero ? Create() : _items.Dequeue();

            item.Enable();
            ReleaseCount++;

            return item;
        }

        private T Create()
        {
            T item = _createFunc.Invoke();
            item.Disable();

            return item;
        }

        public void Return(T item)
        {
            item.ThrowIfNull();
            item.Disable();

            _items.Enqueue(item);
            ReleaseCount--;
        }
    }
}
