using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Code.Tools
{
    public static class UpdateService
    {
        private static readonly HashSet<Action> _registeredMethods = new();

        static UpdateService()
        {
            UpdateRunner updater = new GameObject("UnityUpdateHandler").AddComponent<UpdateRunner>();
            updater.hideFlags = HideFlags.HideAndDontSave;
            Object.DontDestroyOnLoad(updater.gameObject);
        }

        private static event Action OnUpdate;

        public static void Register(Action updateMethod)
        {
            if (_registeredMethods.Add(updateMethod))
            {
                OnUpdate += updateMethod;
            }
        }

        public static void Unregister(Action updateMethod)
        {
            if (_registeredMethods.Remove(updateMethod))
            {
                OnUpdate -= updateMethod;
            }
        }

        private class UpdateRunner : MonoBehaviour
        {
            private void Update()
            {
                OnUpdate?.Invoke();
            }
        }
    }
}
