using UnityEngine;
using XIV.Core.Extensions;

namespace XIV.TweenSystem
{
    internal sealed class MoveTweenX : TweenDriver<float, Transform>
    {
        protected override void OnUpdate(float normalizedTime)
        {
            component.position = component.position.SetX(Mathf.Lerp(startValue, endValue, normalizedTime));
        }

        protected override float GetCurrent()
        {
            return component.position.x;
        }
    }
}