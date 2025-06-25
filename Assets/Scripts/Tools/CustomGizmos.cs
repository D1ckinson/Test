#if UNITY_EDITOR
namespace Assets.Scripts.Tools
{
    using UnityEngine;

    public sealed class CustomGizmos
    {
        private const int CircleSegmentsCount = 32;
        private const int ConeSegmentsCount = 20;
        private const float Half = 0.5f;

        public static void DrawCircle(Vector3 center, float radius, Color? color = null)
        {
            center.ThrowIfNull();
            radius.ThrowIfZeroOrLess();

            Gizmos.color = color == null ? Color.yellow : (Color)color;
            float angleStep = Constants.FullCircleDegrees / CircleSegmentsCount;

            for (int i = Constants.Zero; i < CircleSegmentsCount; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                float nextAngle = (i + Constants.One) * angleStep * Mathf.Deg2Rad;

                Vector3 start = center + new Vector3(Mathf.Cos(angle), Constants.Zero, Mathf.Sin(angle)) * radius;
                Vector3 end = center + new Vector3(Mathf.Cos(nextAngle), Constants.Zero, Mathf.Sin(nextAngle)) * radius;

                Gizmos.DrawLine(start, end);
            }
        }

        public static void DrawCone(Vector3 center, Vector3 direction, float radius, float angle)
        {
            center.ThrowIfNull();
            direction.ThrowIfNotNormalize();
            radius.ThrowIfZeroOrLess();
            angle.ThrowIfZeroOrLess();

            Gizmos.color = Color.red;

            float halfAngle = angle * Half;

            Vector3 rightDirection = Quaternion.Euler(Constants.Zero, halfAngle, Constants.Zero) * direction;
            Vector3 leftDirection = Quaternion.Euler(Constants.Zero, -halfAngle, Constants.Zero) * direction;

            Gizmos.DrawLine(center, center + rightDirection * radius);
            Gizmos.DrawLine(center, center + leftDirection * radius);

            float angleStep = angle / ConeSegmentsCount;
            Vector3 previousPoint = center + leftDirection * radius;

            for (int i = Constants.One; i <= ConeSegmentsCount; i++)
            {
                float currentAngle = -halfAngle + angleStep * i;
                Vector3 nextPoint = center + Quaternion.Euler(Constants.Zero, currentAngle, Constants.Zero) * direction * radius;

                Gizmos.DrawLine(previousPoint, nextPoint);

                previousPoint = nextPoint;
            }
        }
    }
}
#endif
