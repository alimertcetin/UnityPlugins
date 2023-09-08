using UnityEngine;
using XIV_Packages.PCSettingsSystem;

namespace Assets.XIV
{
    public class GraphicSettingManager : SettingManager
    {
        [SerializeField] GraphicPresetItemSO[] presets;
        [SerializeField] int defaultPresetIndex;

        GraphicSettingContainer graphicSettingContainer;

        public override void InitializeContainer()
        {
            graphicSettingContainer = new GraphicSettingContainer(CreateGraphicSettingApplier());
            SettingPreset[] graphicPresets = CreateSettingPresets();
            graphicSettingContainer.AddPresets(graphicPresets);
            graphicSettingContainer.InitializeSettings(graphicPresets[defaultPresetIndex]);
            graphicSettingContainer.ApplyChanges();
            graphicSettingContainer.ClearUndoHistory();
        }

        public override ISettingContainer GetContainer() => graphicSettingContainer;

        static ISettingApplier CreateGraphicSettingApplier()
        {
            ISettingApplier settingApplier = new XIVDefaultSettingApplier();
            settingApplier.AddApplyCommand(new AntialiasingApplyCommand());
            settingApplier.AddApplyCommand(new ResolutionApplyCommand());
            settingApplier.AddApplyCommand(new ShadowQualityApplyCommand());
            settingApplier.AddApplyCommand(new TextureQualityApplyCommand());
            settingApplier.AddApplyCommand(new DisplayTypeApplyCommand());
            settingApplier.AddApplyCommand(new VsyncApplyCommand());
            return settingApplier;
        }

        SettingPreset[] CreateSettingPresets()
        {
            int length = presets.Length;
            var graphicPresets = new SettingPreset[length];

            for (int i = 0; i < length; i++)
            {
                graphicPresets[i] = presets[i].GetGraphicSetting();
            }

            return graphicPresets;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            defaultPresetIndex = defaultPresetIndex < 0 ? 0 : defaultPresetIndex > presets.Length - 1 ? presets.Length - 1 : defaultPresetIndex;
        }
#endif
    }
}