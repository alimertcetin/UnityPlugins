﻿using UnityEngine;

namespace XIV.TweenSystem
{
    internal sealed class MoveTween : TweenDriver<Vector3, Transform>
    {
        protected override void OnUpdate(float normalizedTime)
        {
            component.position = Vector3.Lerp(startValue, endValue, normalizedTime);
        }

        protected override Vector3 GetCurrent()
        {
            return component.position;
        }
    }
}