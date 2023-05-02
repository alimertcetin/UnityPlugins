using UnityEngine;
using XIV.Core.Extensions;

namespace XIV.TweenSystem
{
    internal sealed class ScaleTweenX : TweenDriver<float, Transform>
    {
        protected override void OnUpdate(float normalizedTime)
        {
            component.localScale = component.localScale.SetX(Mathf.Lerp(startValue, endValue, normalizedTime));
        }

        protected override float GetCurrent()
        {
            return component.localScale.x;
        }
    }
}