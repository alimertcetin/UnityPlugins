using System.Collections.Generic;
using UnityEngine;

namespace XIV.TweenSystem
{
    public static class TweenExtensions
    {
        public static bool HasTween(this Component component)
        {
            return XIVTweenSystem.HasTween(component);
        }

        public static void CancelTween(this Component component)
        {
            XIVTweenSystem.CancelTween(component);
        }
        
        public static ScaleTween ScaleTween(this Component component, Vector3 targetScale, float duration)
        {
            var tween = ScaleTweenPool.Get(component.transform, targetScale, duration);
            XIVTweenSystem.AddTween(component, tween);
            return tween;
        }
    }

    public static class ScaleTweenPool
    {
        static List<ScaleTween> pool = new List<ScaleTween>();

        public static ScaleTween Get(Component component, Vector3 targetScale, float duration)
        {
            int count = pool.Count;
            if (count > 0)
            {
                int index = count - 1;
                var tween = pool[index];
                pool.RemoveAt(index);
                return tween.Create(component.transform, targetScale, duration);
            }
            
            return new ScaleTween().Create(component.transform, targetScale, duration);
        }

        public static void Return(ScaleTween scaleTween)
        {
            pool.Add(scaleTween);
        }
    }
}