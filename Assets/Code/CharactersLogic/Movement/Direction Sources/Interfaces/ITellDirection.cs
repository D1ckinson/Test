﻿using System;
using UnityEngine;

namespace Assets.Scripts.Movement
{
    public interface ITellDirection
    {
        public event Action<Vector3> DirectionChanged;

        public void Run();

        public void Stop();
    }
}
