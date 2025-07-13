using Assets.Scripts.Tools;
using UnityEngine;

namespace Assets.Code.Core
{
    public class LevelUpHandler
    {
        public void Handle(int level)
        {
            level.ThrowIfZeroOrLess();
            Time.timeScale = Constants.Zero;

        }
    }
}
