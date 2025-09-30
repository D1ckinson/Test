using System;

namespace Assets.Scripts
{
    public interface ITimeService
    {
        public event Action TimeChanged;

        public void Pause();

        public void Continue();
    }
}