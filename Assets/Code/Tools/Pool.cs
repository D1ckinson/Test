using Assets.Code.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tools
{
    public class Pool<T> where T : MonoBehaviour
    {
        private const int Zero = 0;
        private const int DefaultPreCreatedCount = 10;

        private readonly List<T> _items = new();
        private readonly Func<T> _createFunc;

        public Pool(Func<T> createFunc, int preCreatedCount = DefaultPreCreatedCount)
        {
            _createFunc = createFunc.ThrowIfNull();

            for (int i = Zero; i < preCreatedCount; i++)
            {
                Create();
            }
        }

        public int ReleaseCount => _items.Count(item => item.gameObject.activeSelf);

        public T Get()
        {
            T item = _items.FirstOrDefault(item => item.gameObject.activeSelf == false) ?? Create();
            item.gameObject.SetActive(true);

            return item;
        }

        public void DisableAll()
        {
            _items.ForEach(item => item.SetActive(false));
        }

        private T Create()
        {
            T item = _createFunc.Invoke();
            _items.Add(item);
            item.gameObject.SetActive(false);

            return item;
        }
    }
}
