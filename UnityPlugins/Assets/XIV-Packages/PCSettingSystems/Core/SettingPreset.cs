using System.Collections.Generic;

namespace XIV_Packages.PCSettingsSystem
{
    public readonly struct SettingPreset : ISetting
    {
        public readonly SettingQualityLevel settingQualityLevel;
        public readonly IList<ISetting> presetSettings;

        bool ISetting.canIncludeInPresets => false;
        bool ISetting.IsCritical => true;

        public SettingPreset(SettingPreset setting) : this(setting.settingQualityLevel, setting.presetSettings)
        {

        }

        public SettingPreset(SettingQualityLevel settingQualityLevel, IList<ISetting> presetSettings)
        {
            this.settingQualityLevel = settingQualityLevel;
            this.presetSettings = presetSettings;
        }

        public T GetSetting<T>()
        {
            int count = presetSettings.Count;
            for (int i = 0; i < count; i++)
            {
                if (presetSettings[i] is T setting) return setting;
            }
            return default;
        }

        public static bool IsEqual(SettingPreset preset, IEnumerable<ISetting> collection)
        {
            foreach (var item in collection)
            {
                if (item.canIncludeInPresets && preset.presetSettings.Contains(item) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object other)
        {
            if (other is not SettingPreset settingPreset) return false;
            return IsEqual(settingPreset, presetSettings);
        }

        public override string ToString()
        {
            string presets = "";
            foreach (var setting in presetSettings)
            {
                presets += setting.ToString() + ", ";
            }
            presets = presets.TrimEnd(',');
            return $"{nameof(SettingPreset)}, {nameof(settingQualityLevel)} : {settingQualityLevel}, values : {presets}";
        }
    }
}