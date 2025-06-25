using UnityEngine;

namespace Assets.Scripts.Movement
{
    [CreateAssetMenu(fileName = "NewMovementConfig", menuName = "Game/MovementConfig")]
    internal class MovementConfig : ScriptableObject
    {
        [field: SerializeField][field: Min(5)] internal float MoveSpeed { get; private set; } = 10;
        [field: SerializeField][field: Min(180)] internal float RotationSpeed { get; private set; } = 200;
    }
}