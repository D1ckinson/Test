using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Code.Data
{
    public class GameTimer
    {
        public float PassedTime { get; private set; }

        public void Update()
        {
            PassedTime += Time.deltaTime;
        }

        public void Reset()
        {
            PassedTime = Constants.Zero;
        }
    }
}
