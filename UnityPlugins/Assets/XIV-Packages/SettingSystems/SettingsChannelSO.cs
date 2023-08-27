using System;
using UnityEngine;

namespace TheGame.SettingSystems
{
    public class SettingsChannelSO : ScriptableObject
    {
        Action<Settings> action;

        public void RaiseEvent(Settings settings)
        {
            action.Invoke(settings);
        }
    }
}
