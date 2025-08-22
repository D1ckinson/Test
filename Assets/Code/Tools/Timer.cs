using System;
using UnityEngine;

namespace Assets.Code.Tools
{
    public class Timer
    {
        private const float Zero = 0;

        private float _remainingTime;

        public float Duration { get; private set; }

        public event Action Completed;

        public void Start(float remainingTime)
        {
            if (remainingTime <= Zero)
            {
                throw new ArgumentOutOfRangeException();
            }

            _remainingTime = remainingTime;
            Duration = Zero;
            UpdateService.RegisterUpdate(UpdateTime);
        }

        public void Start()
        {
            Start(float.MaxValue);
        }

        public void Stop()
        {
            UpdateService.UnregisterUpdate(UpdateTime);
            _remainingTime = Zero;
            Duration = Zero;
        }

        public void Pause()
        {
            UpdateService.UnregisterUpdate(UpdateTime);
        }

        public void Continue()
        {
            UpdateService.RegisterUpdate(UpdateTime);
        }

        private void UpdateTime()
        {
            Duration += Time.deltaTime;

            if (Duration >= _remainingTime)
            {
                UpdateService.UnregisterUpdate(UpdateTime);
                Completed?.Invoke();
            }
        }
    }
}
