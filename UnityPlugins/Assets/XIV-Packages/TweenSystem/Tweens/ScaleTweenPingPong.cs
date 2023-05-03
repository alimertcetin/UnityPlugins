using UnityEngine;

namespace XIV.TweenSystem
{
    internal sealed class ScaleTweenPingPong : PingPongTween<Vector3, Transform>
    {
        protected override void Update(float normalizedEasedTime)
        {
            component.localScale = Vector3.Lerp(startValue, endValue, normalizedEasedTime);
        }
    }
}