using System;
using System.Collections.Generic;
using UnityEngine;
using XIV.Core.Utils;

namespace XIV.TweenSystem
{
    public class TweenTest : MonoBehaviour
    {
        public Transform prefab;
        public Vector3 startScale;
        public Vector3 targetScale;
        public float duration;
        public EasingFunction.Ease easingFunc;

        public int testCount;
        
        List<GameObject> goTestList;

        void Awake()
        {
            goTestList = new List<GameObject>();
            var scale = prefab.transform.localScale;
            int horizontal = 5;
            var pos = transform.position;
            for (int i = 0; i < testCount; i++)
            {
                var newGo = Instantiate(prefab.gameObject);
                newGo.transform.position = pos;
                goTestList.Add(newGo);
                pos.x += scale.x;
                
                horizontal--;
                if (horizontal == 0)
                {
                    pos.y += scale.y;
                    pos.x = transform.position.x;
                    horizontal = 5;
                }
            }
        }

        void Update()
        {
            for (int i = 0; i < goTestList.Count; i++)
            {
                if (goTestList[i].transform.HasTween())
                {
                    return;
                }
            }
            
            for (int i = 0; i < goTestList.Count; i++)
            {
                GameObject go = goTestList[i];
                var testTransform = go.transform;
                if (testTransform.HasTween())
                {
                    testTransform.CancelTween();
                }

                testTransform.localScale = startScale;
                testTransform.ScaleTween(targetScale, duration).AddEasing(EasingFunction.GetEasingFunction(easingFunc));
            }
        }

        [ContextMenu(nameof(CancelTween))]
        void CancelTween()
        {
            foreach (GameObject go in goTestList)
            {
                go.transform.CancelTween();
            }
        }
    }
}