using System;
using UnityEngine;
using XIV.Core.Utils;
using XIV_Packages.PoolSystem;

namespace XIV.TweenSystem
{
    public abstract class TweenDriver<TValueType, TComponentType> : TweenDriver<TValueType>, IPoolable where TComponentType : Component
    {
        internal TComponentType component { get; private set; }
        internal IPool pool;

        internal TweenDriver<TValueType, TComponentType> Set(TComponentType component, TValueType startValue, TValueType endValue, float duration)
        {
            base.Set(startValue, endValue, duration);
            this.component = component;
            return this;
        }

        protected override void OnComplete()
        {
            pool.Return(this);
        }

        protected override void OnCancel()
        {
            pool.Return(this);
        }

        void IPoolable.OnPoolCreate(IPool pool)
        {
            this.pool = pool;
        }

        void IPoolable.OnPoolReturn()
        {
            component = default;
        }
    }

    public abstract class TweenDriver<TValueType> : ITween
    {
        protected TValueType startValue;
        protected TValueType endValue;
        protected TValueType currentValue;
        
        protected bool hasCompletedCallback;
        protected bool hasCanceledCallback;
        protected bool useUnscaledTime;
        
        protected EasingFunction.Function easingFunction;
        protected Action onCompleted;
        protected Action onCanceled;

        protected Timer timer;

        static readonly EasingFunction.Function linear;

        static TweenDriver()
        {
            linear = EasingFunction.Linear;
        }

        protected void Set(TValueType startValue, TValueType endValue, float duration)
        {
            Clear();
            this.startValue = startValue;
            this.endValue = endValue;
            timer = new Timer(duration);
        }

        protected abstract void OnUpdate(float normalizedTime);
        protected abstract void OnComplete();
        protected abstract void OnCancel();
        protected abstract TValueType GetCurrent();
        
        float GetTime(float normalizedTime)
        {
            return easingFunction.Invoke(0f, 1f, normalizedTime);
        }

        ITween ITween.AddEasing(EasingFunction.Function easingFunction)
        {
            this.easingFunction = easingFunction;
            return this;
        }

        ITween ITween.UseUnscaledTime()
        {
            useUnscaledTime = true;
            return this;
        }

        ITween ITween.OnComplete(Action action)
        {
            onCompleted += action;
            hasCompletedCallback = action != null;
            return this;
        }

        ITween ITween.OnCanceled(Action action)
        {
            onCanceled = action;
            hasCanceledCallback = action != null;
            return this;
        }

        void ITween.Update(float deltaTime)
        {
            timer.Update(useUnscaledTime ? Time.unscaledTime : Time.deltaTime);
            var easedTime = GetTime(timer.NormalizedTime);
            OnUpdate(easedTime);
            currentValue = GetCurrent();
        }

        bool ITween.IsDone() => timer.IsDone;

        void ITween.Complete()
        {
            OnComplete();
            if (hasCompletedCallback) onCompleted.Invoke();
            
            Clear();
        }

        void ITween.Cancel()
        {
            OnCancel();
            if (hasCanceledCallback) onCanceled.Invoke();
            
            Clear();
        }
        
        void Clear()
        {
            startValue = default;
            endValue = default;
            currentValue = default;
            timer = default;
            hasCompletedCallback = false;
            hasCanceledCallback = false;
            easingFunction = linear;
            onCompleted = null;
            onCanceled = null;
        }
    }
}