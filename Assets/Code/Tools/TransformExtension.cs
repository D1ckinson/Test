using Assets.Scripts.Tools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.State_Machine
{
    public static class TransformExtension
    {
        public static Transform SetAt(this Transform transform, Vector3 position, Vector3 gameAreaCenter, float gameAreaRadius, float offset = 0)
        {
            if (Mathf.Approximately(offset.ThrowIfNegative(), Constants.Zero))
            {
                transform.position = position;

                return transform;
            }

            Vector3 vectorOffset = GenerateOffset();

            if (IsPointInCircle(gameAreaCenter, vectorOffset, gameAreaRadius))
            {
                position += vectorOffset;
            }
            else
            {
                position -= vectorOffset;
            }

            transform.position = position;

            return transform;
        }

        public static Transform LookAt(this Transform transform, Transform target = null)
        {
            Quaternion rotation;

            if (target == null)
            {
                rotation = GenerateRotation();
            }
            else
            {
                Vector3 lookDirection = target.position - transform.position;
                lookDirection.y = Constants.Zero;
                rotation = Quaternion.LookRotation(lookDirection);
            }

            transform.rotation = rotation;

            return transform;
        }

        private static bool IsPointInCircle(Vector3 center, Vector3 point, float radius)
        {
            float sqrDistance = (center - point).sqrMagnitude;
            float sqrRadius = radius * radius;

            return sqrDistance <= sqrRadius;
        }


        private static Vector3 GenerateOffset()
        {
            Vector3 offset = new()
            {
                x = Random.Range(Constants.Zero, Constants.One),
                z = Random.Range(Constants.Zero, Constants.One)
            };

            return offset;
        }

        private static Quaternion GenerateRotation()
        {
            Quaternion rotation = new()
            {
                y = Random.Range(Constants.Zero, Constants.FullCircleDegrees),
            };

            return rotation;
        }
    }
}
