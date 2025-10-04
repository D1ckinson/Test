using UnityEngine;

namespace Assets.Code.Tools
{
    public static class Utilities
    {
        private const int Zero = 0;
        private const int FullCircleDegrees = 360;

        public static Vector3 GenerateRandomDirection()
        {
            float randomAngle = Random.Range(Zero, FullCircleDegrees) * Mathf.Deg2Rad;

            return new(Mathf.Cos(randomAngle), Zero, Mathf.Sin(randomAngle));
        }
    }
}
