using System;
using UnityEngine;
using XIV_Packages.PoolSystem;

namespace XIV.TweenSystem
{
    internal sealed class ScaleTween : TweenDriver<Vector3, Transform>
    {
        protected override void OnUpdate(float normalizedTime)
        {
            component.localScale = Vector3.Lerp(startValue, endValue, normalizedTime);
        }

        protected override Vector3 GetCurrent()
        {
            return component.localScale;
        }
    }

    public abstract class CallbackTween : ITween, IPoolable
    {
        IPool pool;
        
        protected abstract void OnComplete();
        protected abstract void OnCanceled();

        void ITween.Update(float deltaTime)
        {
            
        }
        
        bool ITween.IsDone() => true;
        
        void ITween.Complete()
        {
            OnComplete();
            pool.Return(this);
        }

        void ITween.Cancel()
        {
            OnCanceled();
            pool.Return(this);
        }

        void IPoolable.OnPoolCreate(IPool pool)
        {
            this.pool = pool;
        }

        void IPoolable.OnPoolReturn()
        {
            
        }
    }

    public class OnCompleteCallbackTween : CallbackTween
    {
        Action action;

        public OnCompleteCallbackTween Set(Action action)
        {
            this.action = action;
            return this;
        }
        
        protected override void OnComplete()
        {
            action.Invoke();
        }

        protected override void OnCanceled()
        {
            
        }
    }

    public class OnCanceledCallbackTween : CallbackTween
    {
        Action action;

        public OnCanceledCallbackTween Set(Action action)
        {
            this.action = action;
            return this;
        }
        
        protected override void OnComplete()
        {
            
        }

        protected override void OnCanceled()
        {
            action.Invoke();
        }
    }
}