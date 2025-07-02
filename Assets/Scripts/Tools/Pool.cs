using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    public class Pool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly List<T> _items = new();
        private readonly Func<T> _createFunc;

        public Pool(Func<T> createFunc)//добавить стартовое количество
        {
            createFunc.ThrowIfNull();
            _createFunc = createFunc;
        }

        public int ReleaseCount => _items.Count(item => item.isActiveAndEnabled);

        public T Get()
        {
            T item = _items.FirstOrDefault(item => item.isActiveAndEnabled) ?? Create();
            //T item = _items.Count == Constants.Zero ? Create() : _items.Dequeue();

            item.Enable();
            //ReleaseCount++;

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

            //_items.Enqueue(item);
            //ReleaseCount--;
        }
    }
}
