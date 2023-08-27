using System.Collections.Generic;
using TheGame.ScriptableObjects.Channels;
using TMPro;
using UnityEngine;

namespace XIV_Packages.PCSettingsSystem
{
    public class XIVSettingManager : MonoBehaviour, ISettingListener
    {
        public GraphicPresetItemSO[] presets;
        public int defaultPresetIndex;
        public XIVSettings settings;
        public SettingsChannelSO settingLoadedChannel;
        public TMP_Text txt_Timer;
        public List<string> undoHistory;

        bool hasCriticalChange;
        ISettingContainer settingContiner;
        float timer;
        int lastAppliedState;

        void Awake()
        {
            int length = presets.Length;
            var graphicPresets = new SettingPreset[length];

            for (int i = 0; i < length; i++)
            {
                graphicPresets[i] = presets[i].GetGraphicSetting();
            }

            ISettingApplier settingApplier = new XIVDefaultSettingApplier();
            settingApplier.AddApplyCommand(new GraphicSettingPresetCommand(settingApplier));
            //settingApplier.AddApplyCommand(new AntialiasingCommand());
            //settingApplier.AddApplyCommand(new ResolutionCommand());
            //settingApplier.AddApplyCommand(new ShadowQualityCommand());
            //settingApplier.AddApplyCommand(new TextureQualityCommand());
            //settingApplier.AddApplyCommand(new DisplayTypeCommand());
            //settingApplier.AddApplyCommand(new VsyncCommand());

            var graphicSettingContainer = new GraphicSettingContainer(settingApplier, graphicPresets[defaultPresetIndex]);

            for (int i = 0; i < length; i++)
            {
                if (i == defaultPresetIndex) continue;
                graphicSettingContainer.AddPreset(graphicPresets[i]);
            }
            graphicSettingContainer.ApplyChanges(false);
            graphicSettingContainer.ClearChangeHistory();
            settings = new XIVSettings();
            settings.AddContainer(graphicSettingContainer);
        }

        void Start()
        {
            settingLoadedChannel.RaiseEvent(settings);
        }

        void Update()
        {
            undoHistory.Clear();

            var graphicContainer = settings.GetContainer<GraphicSettingContainer>();

            foreach (var item in graphicContainer.commands.GetUndoOperations())
            {
                undoHistory.Add(item.ToString());
            }


            if (hasCriticalChange == false) return;

            timer += Time.deltaTime;
            txt_Timer.text = timer.ToString("F1");

            if (Input.GetKeyDown(KeyCode.N)) // reject
            {
                RejectChanges();
            }

            if (Input.GetKeyDown(KeyCode.Y)) // accept
            {
                AcceptChanges();
            }

            if (timer > 10)
            {
                RejectChanges();
            }
        }

        void AcceptChanges()
        {
            ResetState();
        }

        void RejectChanges()
        {
            while (settingContiner.Undo())
            {

            }
            ResetState();
        }

        void ResetState()
        {
            settingContiner = default;
            hasCriticalChange = false;
            timer = 0f;
            txt_Timer.text = timer.ToString("F1");
        }

        void OnEnable()
        {
            int containerCount = settings.containerCount;
            for (int i = 0; i < containerCount; i++)
            {
                settings.GetContainerAt(i).AddListener(this);
            }
        }

        void OnDisable()
        {
            int containerCount = settings.containerCount;
            for (int i = 0; i < containerCount; i++)
            {
                settings.GetContainerAt(i).RemoveListener(this);
            }
        }

        void ISettingListener.OnSettingChanged(SettingChange _) { }

        void ISettingListener.OnBeforeApply(ISettingContainer container)
        {
            if (settingContiner != default) return;

            foreach (SettingChange settingchange in container.GetUnappliedSettings())
            {
                if (settingchange.to.IsCritical)
                {
                    settingContiner = container;
                    hasCriticalChange = true;
                    Debug.Log(nameof(settingchange.to) + " = " + settingchange.to);
                    break;
                }
            }
        }

        void ISettingListener.OnAfterApply(ISettingContainer _) { }

#if UNITY_EDITOR
        void OnValidate()
        {
            defaultPresetIndex = defaultPresetIndex < 0 ? 0 : defaultPresetIndex > presets.Length - 1 ? presets.Length - 1 : defaultPresetIndex;
        }
#endif

    }
}