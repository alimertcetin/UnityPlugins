using System;
using System.Collections.Generic;
using UnityEngine;
using XIV.Core.Utils;
using XIV_Packages.PoolSystem;

namespace XIV.TweenSystem
{
    internal interface ITweenContainer
    {
        void Add(ITween tween);
        void Clear();
        void Start();
        ITween AddEasing(EasingFunction.Function easingFunc);
        ITween UseUnscaledTime();
        ITween OnComplete(Action action);
        ITween OnCanceled(Action action);
    }
    
    public sealed class XIVTween : ITweenContainer, IPoolable
    {
        internal Component component;
        static List<ITween> tweenBuffer = new List<ITween>(4);
        static ITween current;
        IPool pool;

        void ITweenContainer.Add(ITween tween)
        {
            tweenBuffer.Add(tween);
            current = tween;
        }

        void ITweenContainer.Clear()
        {
            tweenBuffer.Clear();
        }

        void ITweenContainer.Start()
        {
            for (int i = 0; i < tweenBuffer.Count; i++)
            {
                XIVTweenSystem.AddTween(component, tweenBuffer[i]);
            }
            pool.Return(this);
        }

        ITween ITweenContainer.AddEasing(EasingFunction.Function easingFunc)
        {
            current.AddEasing(easingFunc);
            return current;
        }

        ITween ITweenContainer.UseUnscaledTime()
        {
            current.UseUnscaledTime();
            return current;
        }

        ITween ITweenContainer.OnComplete(Action action)
        {
            current.OnComplete(action);
            return current;
        }

        ITween ITweenContainer.OnCanceled(Action action)
        {
            for (int i = 0; i < tweenBuffer.Count; i++)
            {
                tweenBuffer[i].OnCanceled(action);
            }
            return current;
        }

        void IPoolable.OnPoolCreate(IPool pool)
        {
            this.pool = pool;
        }

        void IPoolable.OnPoolReturn()
        {
            component = default;
            current = default;
        }
    }
}