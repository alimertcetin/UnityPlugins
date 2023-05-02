using System;
using UnityEngine;
using XIV.Core.Utils;
using XIV_Packages.PoolSystem;

namespace XIV.TweenSystem
{
    public static class XIVTweenExtensions
    {
        public static bool HasTween(this Component component)
        {
            return XIVTweenSystem.HasTween(component);
        }

        public static void CancelTween(this Component component)
        {
            XIVTweenSystem.CancelTween(component);
        }
        
        static T GetTween<T>() where T : IPoolable
        {
            return XIVPoolSystem.HasPool<T>() ? XIVPoolSystem.GetItem<T>() : XIVPoolSystem.AddPool(new XIVPool<T>(Activator.CreateInstance<T>)).GetItem();
        }

        public static XIVTween XIVTween(this Component component)
        {
            var tween = GetTween<XIVTween>();
            ((ITweenContainer)tween).Clear();
            tween.component = component;
            return tween;
        }

        public static void Start(this XIVTween tween)
        {
            ((ITweenContainer)tween).Start();
        }

        public static XIVTween AddEasing(this XIVTween tween, EasingFunction.Function easingFunc)
        {
            ((ITweenContainer)tween).AddEasing(easingFunc);
            return tween;
        }

        public static XIVTween OnComplete(this XIVTween tween, Action action)
        {
            ((ITweenContainer)tween).OnComplete(action);
            return tween;
        }

        public static XIVTween OnCanceled(this XIVTween tween, Action action)
        {
            ((ITweenContainer)tween).OnCanceled(action);
            return tween;
        }

        public static XIVTween UseUnscaledTime(this XIVTween tween)
        {
            ((ITweenContainer)tween).UseUnscaledTime();
            return tween;
        }

        public static XIVTween MoveTo(this XIVTween tween, Vector3 to, float duration)
        {
            var from = tween.component.transform.position;
            return Move(tween, from, to, duration);
        }

        public static XIVTween ScaleTo(this XIVTween tween, Vector3 to, float duration)
        {
            var from = tween.component.transform.localScale;
            return Scale(tween, from, to, duration);
        }

        public static XIVTween Scale(this XIVTween tween, Vector3 from, Vector3 to, float duration)
        {
            var t = GetTween<ScaleTween>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
        
        public static XIVTween ScaleX(this XIVTween tween, float from, float to, float duration)
        {
            var t = GetTween<ScaleTweenX>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
        
        public static XIVTween ScaleY(this XIVTween tween, float from, float to, float duration)
        {
            var t = GetTween<ScaleTweenY>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
        
        public static XIVTween ScaleZ(this XIVTween tween, float from, float to, float duration)
        {
            var t = GetTween<ScaleTweenZ>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
        
        public static XIVTween Move(this XIVTween tween, Vector3 from, Vector3 to, float duration)
        {
            var t = GetTween<MoveTween>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
        
        public static XIVTween MoveX(this XIVTween tween, float from, float to, float duration)
        {
            var t = GetTween<MoveTweenZ>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
        
        public static XIVTween MoveY(this XIVTween tween, float from, float to, float duration)
        {
            var t = GetTween<MoveTweenY>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
        
        public static XIVTween MoveZ(this XIVTween tween, float from, float to, float duration)
        {
            var t = GetTween<MoveTweenZ>().Set(tween.component.transform, from, to, duration);
            ((ITweenContainer)tween).Add(t);
            return tween;
        }
    }
}