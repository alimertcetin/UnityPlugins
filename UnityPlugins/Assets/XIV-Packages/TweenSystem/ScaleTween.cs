using UnityEngine;
using XIV.Core.Utils;

namespace XIV.TweenSystem
{
    public class ScaleTween : TweenBase<ScaleTween>, ITween
    {
        Transform transform;
        Vector3 initialScale;
        Vector3 targetScale;
        Timer timer;
        
        public ScaleTween()
        {
            base.tween = this;
        }

        public ScaleTween Create(Transform transform, Vector3 targetScale, float duration)
        {
            this.transform = transform;
            this.initialScale = this.transform.localScale;
            this.targetScale = targetScale;
            this.timer = new Timer(duration);
            return this;
        }
        
        void ITween.Update(float deltaTime)
        {
            timer.Update(deltaTime);
            transform.localScale = Vector3.Lerp(initialScale, targetScale, GetTime(timer.NormalizedTime));
        }

        bool ITween.IsDone()
        {
            return timer.IsDone;
        }

        void ITween.Complete()
        {
            base.Complete();
            ScaleTweenPool.Return(this);
        }

        void ITween.Cancel()
        {
            base.Cancel();
            ScaleTweenPool.Return(this);
        }
        
    }
}