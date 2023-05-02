using UnityEngine;
using XIV.Core.Extensions;

namespace XIV.TweenSystem
{
    internal sealed class ScaleTweenZ : TweenDriver<float, Transform>
    {
        protected override void OnUpdate(float normalizedTime)
        {
            component.localScale = component.localScale.SetZ(Mathf.Lerp(startValue, endValue, normalizedTime));
        }

        protected override float GetCurrent()
        {
            return component.localScale.x;
        }
    }
}