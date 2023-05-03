using UnityEngine;

namespace XIV.TweenSystem
{
    internal sealed class MoveTweenPingPong : PingPongTween<Vector3, Transform>
    {
        protected override void Update(float normalizedEasedTime)
        {
            component.position = Vector3.Lerp(startValue, endValue, normalizedEasedTime);
        }
    }
}