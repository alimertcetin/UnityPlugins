using System;
using System.Collections.Generic;
using UnityEngine;
using XIV.Core.Utils;
using XIV_Packages.PoolSystem;

namespace XIV.TweenSystem
{
    public sealed class XIVTweenFactory
    {
        static readonly XIVTweenFactory instance = new XIVTweenFactory();
        static readonly List<TweenTimeline> timelineBuffer = new List<TweenTimeline>(4);

        Component component;
        TweenTimeline currentTimeline;
        int componentInstanceID;
        bool useCurrent;
        
        static T GetPooledTween<T>() where T : IPoolable
        {
            return XIVPoolSystem.HasPool<T>() ? XIVPoolSystem.GetItem<T>() : XIVPoolSystem.AddPool(new XIVPool<T>(Activator.CreateInstance<T>)).GetItem();
        }

        public static XIVTweenFactory GetTween(Component component)
        {
            instance.Setup(component);
            return instance;
        }

        void Setup(Component component)
        {
            this.component = component;
            this.componentInstanceID = component.gameObject.GetInstanceID();
            this.currentTimeline = TweenTimeline.GetTimeline();
            useCurrent = true;
        }

        void Clear()
        {
            component = default;
            currentTimeline = default;
            timelineBuffer.Clear();
        }

        public XIVTweenFactory AddTween(ITween tween)
        {
            if (useCurrent == false)
            {
                timelineBuffer.Add(currentTimeline);
                currentTimeline = TweenTimeline.GetTimeline(tween);
                return this;
            }

            currentTimeline.tweens.Add() = tween;
            useCurrent = false;
            return this;
        }

        public void Start()
        {
            timelineBuffer.Add(currentTimeline);
            int count = timelineBuffer.Count;
            for (int i = 0; i < count; i++)
            {
                XIVTweenSystem.AddTween(componentInstanceID, timelineBuffer[i]);
            }
            Clear();
        }

        public XIVTweenFactory And()
        {
            useCurrent = true;
            return this;
        }

        public XIVTweenFactory OnComplete(Action action)
        {
            var t = GetPooledTween<OnCompleteCallbackTween>().Set(action);
            return AddTween(t);
        }

        public XIVTweenFactory OnCanceled(Action action)
        {
            var t = GetPooledTween<OnCanceledCallbackTween>().Set(action);
            return AddTween(t);
        }

        public XIVTweenFactory Scale(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<ScaleTween>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }

        public XIVTweenFactory ScaleX(float from, float to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<ScaleTweenX>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }
        
        public XIVTweenFactory ScaleY(float from, float to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<ScaleTweenY>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }
        
        public XIVTweenFactory ScaleZ(float from, float to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<ScaleTweenZ>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }
        
        public XIVTweenFactory Move(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<MoveTween>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }
        
        public XIVTweenFactory MoveX(float from, float to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<MoveTweenZ>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }
        
        public XIVTweenFactory MoveY(float from, float to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<MoveTweenY>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }
        
        public XIVTweenFactory MoveZ(float from, float to, float duration, EasingFunction.Function easingFunc)
        {
            var t = GetPooledTween<MoveTweenZ>().Set(component.transform, from, to, duration, easingFunc);
            return AddTween(t);
        }
    }
}