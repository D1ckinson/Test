using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Code.Ui
{
    public class Timer
    {
        private float _remainingTime;

        public event Action OnTimerComplete;

        public void Start(float duration)
        {
            _remainingTime = duration.ThrowIfZeroOrLess();

            UpdateService.Register(Update);
        }

        private void Update()
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime <= 0)
            {
                UpdateService.Unregister(Update);
                OnTimerComplete?.Invoke();
            }
        }
    }
}
