using System;
using DG.Tweening;
using UnityEngine;

namespace Features.Movement
{
    [Serializable]
    public struct MovementSpeed
    {
        [Min(0f)]
        public float Speed;
        public Ease Ease;
    }
}