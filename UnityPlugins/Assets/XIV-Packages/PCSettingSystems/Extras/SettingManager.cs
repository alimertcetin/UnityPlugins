using UnityEngine;

namespace Assets.XIV
{
    public abstract class SettingManager : MonoBehaviour
    {
        public abstract void InitializeContainer();
        public abstract ISettingContainer GetContainer();
    }
}