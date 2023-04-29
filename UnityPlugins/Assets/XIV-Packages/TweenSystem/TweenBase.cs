using System;
using XIV.Core.Utils;

namespace XIV.TweenSystem
{
    public abstract class TweenBase
    {
    }

    public abstract class TweenBase<T> : TweenBase where T : ITween
    {
        protected T tween;
        
        protected bool hasFinishedCallback;
        protected bool hasCanceledCallback;
        
        protected EasingFunction.Function easingFunction;
        protected Action onFinished;
        protected Action onCanceled;

        static readonly EasingFunction.Function linear;

        static TweenBase()
        {
            linear = EasingFunction.Linear;
        }

        public TweenBase()
        {
            easingFunction = linear;
        }

        protected float GetTime(float normalizedTime)
        {
            return easingFunction.Invoke(0f, 1f, normalizedTime);
        }

        public T AddEasing(EasingFunction.Function easingFunction)
        {
            this.easingFunction = easingFunction;
            return tween;
        }

        public T OnFinished(Action action)
        {
            onFinished = action;
            hasFinishedCallback = action != null;
            return tween;
        }

        public T OnCanceled(Action action)
        {
            onCanceled = action;
            hasCanceledCallback = action != null;
            return tween;
        }

        protected void Complete()
        {
            if (hasFinishedCallback) onFinished.Invoke();
            
            Clear();
        }

        protected void Cancel()
        {
            if (hasCanceledCallback) onCanceled.Invoke();
            
            Clear();
        }

        void Clear()
        {
            hasFinishedCallback = false;
            hasCanceledCallback = false;
            easingFunction = linear;
            onFinished = null;
            onCanceled = null;
        }
    }
}