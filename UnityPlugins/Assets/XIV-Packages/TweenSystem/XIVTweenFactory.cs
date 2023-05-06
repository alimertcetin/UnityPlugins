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
            instance.Initialize(component);
            return instance;
        }

        void Initialize(Component component)
        {
            this.component = component;
            this.componentInstanceID = component.gameObject.GetInstanceID();
            this.currentTimeline = TweenTimeline.GetTimeline();
            useCurrent = true;
        }

        void Reset()
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

        XIVTweenFactory AddTween<T>(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0) where T : TweenDriver<Vector3, Transform>
        {
            var t = GetPooledTween<T>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount);
            return AddTween(t);
        }

        XIVTweenFactory AddTween<T>(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0) where T : TweenDriver<float, Transform>
        {
            var t = GetPooledTween<T>().Set(component.transform, from, to, duration, easingFunc, isPingPong, loopCount);
            return AddTween(t);
        }

        public void Start()
        {
            timelineBuffer.Add(currentTimeline);
            int count = timelineBuffer.Count;
            for (int i = 0; i < count; i++)
            {
                XIVTweenSystem.AddTween(componentInstanceID, timelineBuffer[i]);
            }
            Reset();
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

        public XIVTweenFactory Scale(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<ScaleTween>(from, to, duration, easingFunc, isPingPong, loopCount);
        }

        public XIVTweenFactory ScaleX(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<ScaleTweenX>(from, to, duration, easingFunc, isPingPong, loopCount);
        }
        
        public XIVTweenFactory ScaleY(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<ScaleTweenY>(from, to, duration, easingFunc, isPingPong, loopCount);
        }
        
        public XIVTweenFactory ScaleZ(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<ScaleTweenZ>(from, to, duration, easingFunc, isPingPong, loopCount);
        }
        
        public XIVTweenFactory Move(Vector3 from, Vector3 to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<MoveTween>(from, to, duration, easingFunc, isPingPong, loopCount);
        }
        
        public XIVTweenFactory MoveX(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<MoveTweenX>(from, to, duration, easingFunc, isPingPong, loopCount);
        }
        
        public XIVTweenFactory MoveY(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<MoveTweenY>(from, to, duration, easingFunc, isPingPong, loopCount);
        }
        
        public XIVTweenFactory MoveZ(float from, float to, float duration, EasingFunction.Function easingFunc, bool isPingPong = false, int loopCount = 0)
        {
            return AddTween<MoveTweenZ>(from, to, duration, easingFunc, isPingPong, loopCount);
        }
    }
}