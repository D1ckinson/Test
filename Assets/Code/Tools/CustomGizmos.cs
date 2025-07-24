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

            Gizmos.color = color ?? Color.yellow;
            float angleStep = Constants.FullCircleDegrees / CircleSegmentsCount;

            Vector3 startPoint = CalculatePoint(Constants.Zero);

            for (int i = Constants.One; i <= CircleSegmentsCount + Constants.One; i++)
            {
                float angle = i * angleStep * Mathf.Deg2Rad;
                Vector3 endPoint = CalculatePoint(angle);

                Gizmos.DrawLine(startPoint, endPoint);
                startPoint = endPoint;
            }

            Vector3 CalculatePoint(float angle)
            {
                Vector3 point = center + new Vector3(Mathf.Cos(angle), Constants.Zero, Mathf.Sin(angle)) * radius;

                return point;
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
