using System.Collections.Generic;
using UnityEngine;
using XIV.Core.Collections;
using XIV_Packages.PoolSystem;

namespace XIV.TweenSystem
{
    internal static class XIVTweenSystem
    {
        class TweenData : IPoolable
        {
            public int instanceID;
            public DynamicArray<ITween> tweens;
            IPool pool;

            public TweenData()
            {
                tweens = new DynamicArray<ITween>(2);
            }

            public void Return()
            {
                pool.Return(this);
            }

            void IPoolable.OnPoolCreate(IPool pool)
            {
                this.pool = pool;
            }

            void IPoolable.OnPoolReturn()
            {
                instanceID = -1;
                tweens.Clear();
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
                        tweenLookup.Remove(tweenData.instanceID);
                        tweenData.Return();
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

        static HashSet<int> tweenLookup = new HashSet<int>();

        internal static void AddTween(Component component, ITween tween)
        {
            var instanceID = component.gameObject.GetInstanceID();
            var tweenData = GetTweenData(instanceID);
            tweenData.tweens.Add() = tween;
        }

        internal static void CancelTween(Component component)
        {
            var instanceID = component.gameObject.GetInstanceID();
            if (tweenLookup.Contains(instanceID) == false) return;

            int index = IndexOfTweenData(instanceID);
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
            return tweenLookup.Contains(component.gameObject.GetInstanceID());
        }

        static TweenData GetTweenData(int instanceID)
        {
            if (tweenLookup.Contains(instanceID))
            {
                return Helper.tweenDatas[IndexOfTweenData(instanceID)];
            }

            var tweenData = XIVPoolSystem.HasPool<TweenData>() ? XIVPoolSystem.GetItem<TweenData>() : XIVPoolSystem.AddPool(new XIVPool<TweenData>(() => new TweenData())).GetItem();
            tweenData.instanceID = instanceID;
            tweenLookup.Add(instanceID);
            Helper.tweenDatas.Add(tweenData);
            return tweenData;
        }

        static int IndexOfTweenData(int instanceID)
        {
            var tweenDatas = Helper.tweenDatas;
            int count = tweenDatas.Count;
            for (int i = 0; i < count; i++)
            {
                if (tweenDatas[i].instanceID == instanceID) return i;
            }

            return -1;
        }
    }
}
