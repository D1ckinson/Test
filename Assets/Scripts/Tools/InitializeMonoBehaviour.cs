using UnityEngine;

namespace Assets.Scripts.Tools
{
    public class InitializeMonoBehaviour : MonoBehaviour//
    {
        protected bool IsInitialized { get; private set; } = false;

        public void Initialize()
        {
            IsInitialized = true;
        }
    }
}
