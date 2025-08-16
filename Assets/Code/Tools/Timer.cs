using Assets.Scripts.Tools;
using System;
using UnityEngine;

namespace Assets.Code.Tools
{
    public class Timer
    {
        private int _duration;
        private float _remainingTime;

        public event Action Completed;

        public void Start(int duration)
        {
            _duration = duration.ThrowIfZeroOrLess();
            _remainingTime = _duration;

            UpdateService.Register(Update);
        }

        private void Update()
        {
            _remainingTime -= Time.deltaTime;

            if (_remainingTime <= Constants.Zero)
            {
                UpdateService.Unregister(Update);
                Completed?.Invoke();
            }
        }

        public void Pause()
        {
            UpdateService.Unregister(Update);
        }

        public void Continue()
        {
            UpdateService.Register(Update);
        }
    }
}
