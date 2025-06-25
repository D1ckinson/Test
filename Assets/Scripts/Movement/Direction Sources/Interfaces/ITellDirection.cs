using System;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    internal interface ITellDirection
    {
        public event Action<Vector3> DirectionChanged;
    }
}
