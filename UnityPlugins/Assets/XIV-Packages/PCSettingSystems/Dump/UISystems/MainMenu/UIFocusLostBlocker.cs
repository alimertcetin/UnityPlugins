﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace XIV_Packages.PCSettingSystems
{
    public class UIFocusLostBlocker : MonoBehaviour
    {
        GameObject lastSelected;

        void Update()
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                lastSelected = EventSystem.current.currentSelectedGameObject;
                return;
            }

            if (lastSelected && lastSelected.gameObject.activeSelf && EventSystem.current.currentSelectedGameObject != lastSelected)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
        }
    }
}