using Assets.Code.Tools;
using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class TimeService : ITimeService
    {
        public event Action TimeChanged;

        public void Pause()
        {
            Time.timeScale = Constants.Zero;
            TimeChanged?.Invoke();
        }

        public void Continue()
        {
            Time.timeScale = Constants.One;
            TimeChanged?.Invoke();
        }
    }
}
