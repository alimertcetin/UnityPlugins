using System;
using UnityEngine;
using XIV.Core.Utils;
using XIV_Packages.PoolSystem;

namespace XIV.TweenSystem
{
    public abstract class TweenDriver<TValueType, TComponentType> : TweenDriver<TValueType> where TComponentType : Component
    {
        internal TComponentType component { get; private set; }

        public TweenDriver<TValueType, TComponentType> Set(TComponentType component, TValueType startValue, TValueType endValue, float duration, EasingFunction.Function easingFunction)
        {
            base.Set(startValue, endValue, duration, easingFunction);
            this.component = component;
            return this;
        }

        protected override void OnComplete()
        {
            component = default;
        }

        protected override void OnCancel()
        {
            component = default;
        }
    }

    public abstract class TweenDriver<TValueType> : ITween, IPoolable
    {
        protected TValueType startValue;
        protected TValueType endValue;
        protected EasingFunction.Function easingFunction;
        protected Timer timer;
        IPool pool;
        bool hasPool;

        public TweenDriver<TValueType> Set(TValueType startValue, TValueType endValue, float duration, EasingFunction.Function easingFunction)
        {
            Clear();
            this.startValue = startValue;
            this.endValue = endValue;
            this.easingFunction = easingFunction;
            timer = new Timer(duration);
            return this;
        }

        protected abstract void OnUpdate(float easedTime);
        protected abstract void OnComplete();
        protected abstract void OnCancel();
        
        void Clear()
        {
            startValue = default;
            endValue = default;
            timer = default;
        }

        void ITween.Update(float deltaTime)
        {
            timer.Update(deltaTime);
            var easedTime = easingFunction.Invoke(0f, 1f, timer.NormalizedTime);
            OnUpdate(easedTime);
        }

        bool ITween.IsDone() => timer.IsDone;

        void ITween.Complete()
        {
            OnComplete();
            if (hasPool) pool.Return(this);
        }

        void ITween.Cancel()
        {
            OnCancel();
            if (hasPool) pool.Return(this);
        }

        void IPoolable.OnPoolCreate(IPool pool)
        {
            this.pool = pool;
            this.hasPool = pool != default;
        }

        void IPoolable.OnPoolReturn()
        {
            Clear();
        }
    }
}