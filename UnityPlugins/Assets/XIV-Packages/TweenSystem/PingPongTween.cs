using UnityEngine;

namespace XIV.TweenSystem
{
    internal abstract class PingPongTween<TValue, TComponent> : TweenDriver<TValue, TComponent> where TComponent : Component
    {
        bool reverse;
        protected abstract void Update(float normalizedEasedTime);
        
        protected override void OnUpdate(float easedTime)
        {
            if (reverse == false && easedTime > 0.5f)
            {
                reverse = true;
                (startValue, endValue) = (endValue, startValue);
            }
            easedTime = easedTime > 0.5f ? (easedTime - 0.5f) / 0.5f : easedTime / 0.5f;
            Update(easedTime);
        }

        protected override void OnCancel()
        {
            base.OnCancel();
            reverse = false;
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            reverse = false;
        }
    }
}