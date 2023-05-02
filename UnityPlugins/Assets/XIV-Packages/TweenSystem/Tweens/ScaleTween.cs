using UnityEngine;

namespace XIV.TweenSystem
{
    internal sealed class ScaleTween : TweenDriver<Vector3, Transform>
    {
        protected override void OnUpdate(float normalizedTime)
        {
            component.localScale = Vector3.Lerp(startValue, endValue, normalizedTime);
        }

        protected override Vector3 GetCurrent()
        {
            return component.localScale;
        }
    }
}