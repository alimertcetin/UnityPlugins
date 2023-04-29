using System.Collections.Generic;
using UnityEngine;
using XIV.Core.Collections;

namespace XIV.TweenSystem
{
    internal static class XIVTweenSystem
    {
        class TweenData
        {
            public GameObject gameObject;
            public DynamicArray<ITween> tweens;

            public TweenData()
            {
                tweens = new DynamicArray<ITween>(2);
            }
            
        }

        static class TweenDataPool
        {
            static Queue<TweenData> tweenDatas = new Queue<TweenData>();

            internal static TweenData Get()
            {
                var tweenData = tweenDatas.Count > 0 ? tweenDatas.Dequeue() : new TweenData();
                return tweenData;
            }

            internal static void Return(TweenData tweenData)
            {
#if UNITY_EDITOR
                if (tweenData == null) throw new System.NullReferenceException("tweenData is null");
#endif
                tweenData.gameObject = null;
                tweenData.tweens.Clear();
                tweenDatas.Enqueue(tweenData);
            }
        }
        
        class TweenHelperMono : MonoBehaviour
        {
            internal List<TweenData> tweenDatas = new List<TweenData>();

            void Update()
            {
                int count = tweenDatas.Count;
                for (int i = count - 1; i >= 0; i--)
                {
                    TweenData tweenData = tweenDatas[i];
                    int tweenCount = tweenData.tweens.Count;
                    for (int j = tweenCount - 1; j >= 0; j--)
                    {
                        ITween tween = tweenData.tweens[j];
                        tween.Update(Time.deltaTime);
                        if (tween.IsDone())
                        {
                            tween.Complete();
                            tweenData.tweens.RemoveAt(j);
                        }
                    }

                    if (tweenData.tweens.Count == 0)
                    {
                        tweenDatas.RemoveAt(i);
                        TweenDataPool.Return(tweenData);
                    }
                }
            }
        }
        
        static TweenHelperMono helper;

        static TweenHelperMono Helper
        {
            get
            {
                if (helper == null) helper = new GameObject("XIV-TweenSystem-Helper").AddComponent<TweenHelperMono>();
                return helper;
            }
        }
        
        internal static void AddTween(Component component, ITween tween)
        {
            GetTweenData(component).tweens.Add() = tween;
        }

        internal static void CancelTween(Component component)
        {
            int index = IndexOfTweenData(component);
            if (index < 0) return;

            var tweenDatas = Helper.tweenDatas;
            var tweenData = tweenDatas[index];
            tweenDatas.RemoveAt(index);

            int count = tweenData.tweens.Count;
            for (int i = 0; i < count; i++)
            {
                tweenData.tweens[i].Cancel();
            }
        }

        internal static bool HasTween(Component component)
        {
            return IndexOfTweenData(component) > -1;
        }

        static TweenData GetTweenData(Component component)
        {
            int index = IndexOfTweenData(component);
            if (index > -1)
            {
                return Helper.tweenDatas[index];
            }

            var tweenData = TweenDataPool.Get();
            tweenData.gameObject = component.gameObject;
            Helper.tweenDatas.Add(tweenData);
            return tweenData;
        }

        static int IndexOfTweenData(Component component)
        {
            var tweenDatas = Helper.tweenDatas;
            var go = component.gameObject;
            int count = tweenDatas.Count;
            for (int i = 0; i < count; i++)
            {
                if (tweenDatas[i].gameObject == go) return i;
            }

            return -1;
        }
    }
}
