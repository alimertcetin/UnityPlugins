using UnityEngine;

namespace TheGame.UISystems.Components
{
    public class XIVSettingDropdown : MonoBehaviour
    {
        public XIVColoredObjectDropdown dropdown;

#if UNITY_EDITOR

        void OnValidate()
        {
            if (dropdown) return;
            dropdown = GetComponentInChildren<XIVColoredObjectDropdown>();
        }
#endif

    }

}