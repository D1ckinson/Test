using UnityEngine;
using Assets.Scripts.Tools;
using System;
using Assets.Scripts.CollectorsSystem;

namespace Assets.Scripts.ExperienceSystem
{
    public class Experience : BaseCollectable<Experience>
    {
        [field: SerializeField][field: Min(1)] public int Value { get; private set; } = 10;
    }
}
