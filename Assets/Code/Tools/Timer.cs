using Assets.Code.Tools;
using Assets.Scripts.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Ui
{
    public class Timer
    {
        private readonly List<TimerData> _activeTimers = new();

        public Timer Start(float duration, Action onComplete, bool isRepeated = false)
        {
            _activeTimers.Add(new(duration, onComplete, isRepeated));

            if (_activeTimers.Count == Constants.One)
            {
                UpdateService.Register(Update);
            }

            return this;
        }

        public void Stop(Action onComplete)
        {
            TimerData data = _activeTimers.First(data => data.Callback == onComplete);
            _activeTimers.Remove(data);
        }

        private void Update()
        {
            for (int i = _activeTimers.LastIndex(); i >= Constants.Zero; i--)
            {
                TimerData timer = _activeTimers[i];
                timer.RemainingTime -= Time.deltaTime;

                if (timer.RemainingTime > Constants.Zero)
                {
                    continue;
                }

                timer.Callback?.Invoke();

                if (timer.IsRepeated)
                {
                    continue;
                }

                _activeTimers.Remove(timer);
            }

            if (_activeTimers.Count == Constants.Zero)
            {
                UpdateService.Unregister(Update);
            }
        }

        private class TimerData
        {
            public float RemainingTime;
            public Action Callback;
            public bool IsRepeated;

            public TimerData(float remainingTime, Action callback, bool isRepeated)
            {
                RemainingTime = remainingTime.ThrowIfZeroOrLess();
                Callback = callback.ThrowIfNull();
                IsRepeated = isRepeated;
            }
        }
    }
}
