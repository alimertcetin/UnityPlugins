using System;
using System.Collections.Generic;
using TheGame.UISystems.Components;
using TheGame.UISystems.TabSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using UnityEngine.UI;
using XIV_Packages.PCSettingsSystem;

namespace TheGame.UISystems.MainMenu
{
    public class GraphicSettingsTabPageUI : TabPageUI, ISettingListener
    {
        [SerializeField] ScriptableObjects.Channels.SettingsChannelSO settingsLoaded;
        
        [Header("UI Elements for Graphic Settings")]
        [SerializeField] XIVSettingDropdown presetDropdown;
        [SerializeField] XIVSettingDropdown resolutionDropdown;
        [SerializeField] XIVSettingDropdown displayTypeDropdown;
        [SerializeField] SettingToggle vsyncToggle;
        [SerializeField] XIVSettingDropdown antiAliasDropdown;
        [SerializeField] XIVSettingDropdown shadowQualityDropdown;
        [SerializeField] XIVSettingDropdown textureQualityDropdown;

        GraphicSettingContainer graphicSettingContainer;
        Dictionary<Type, Action<ISetting>> onSettingChangeLookup = new Dictionary<Type, Action<ISetting>>();

        protected override void Awake()
        {
            base.Awake();
            onSettingChangeLookup.Add(typeof(SettingPreset), UpdatePresetDropdown);
            onSettingChangeLookup.Add(typeof(VsyncSetting), UpdateVsyncToggle);
            onSettingChangeLookup.Add(typeof(AntiAliasingSetting), UpdateAntiAliasingDropdown);
            onSettingChangeLookup.Add(typeof(ShadowQualitySetting), UpdateShadowQualityDropdown);
            onSettingChangeLookup.Add(typeof(TextureQualitySetting), UpdateTextureQualityDropdown);
            onSettingChangeLookup.Add(typeof(ResolutionSetting), UpdateResolutionDropdown);
            onSettingChangeLookup.Add(typeof(DisplayTypeSetting), UpdateDisplayTypeDropdown);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                graphicSettingContainer.ApplyChanges(true);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                graphicSettingContainer.Undo();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                graphicSettingContainer.Redo();
            }
        }

        void OnEnable()
        {
            settingsLoaded.Register(OnSettingsLoaded);
            presetDropdown.dropdown.onValueChanged.AddListener(OnPresetValueChanged);
            resolutionDropdown.dropdown.onValueChanged.AddListener(OnResolutionValueChanged);
            displayTypeDropdown.dropdown.onValueChanged.AddListener(OnDisplayTypeValueChanged);
            vsyncToggle.toggle.onValueChanged.AddListener(OnVsyncValueChanged);
            antiAliasDropdown.dropdown.onValueChanged.AddListener(OnAntiAliasValueChanged);
            shadowQualityDropdown.dropdown.onValueChanged.AddListener(OnShadowQualityValueChanged);
            textureQualityDropdown.dropdown.onValueChanged.AddListener(OnTextureQualityValueChanged);
            this.graphicSettingContainer?.AddListener(this);
        }

        void OnDisable()
        {
            settingsLoaded.Unregister(OnSettingsLoaded);
            presetDropdown.dropdown.onValueChanged.RemoveListener(OnPresetValueChanged);
            resolutionDropdown.dropdown.onValueChanged.RemoveListener(OnResolutionValueChanged);
            displayTypeDropdown.dropdown.onValueChanged.RemoveListener(OnDisplayTypeValueChanged);
            vsyncToggle.toggle.onValueChanged.RemoveListener(OnVsyncValueChanged);
            antiAliasDropdown.dropdown.onValueChanged.RemoveListener(OnAntiAliasValueChanged);
            shadowQualityDropdown.dropdown.onValueChanged.RemoveListener(OnShadowQualityValueChanged);
            textureQualityDropdown.dropdown.onValueChanged.RemoveListener(OnTextureQualityValueChanged);
            this.graphicSettingContainer?.RemoveListener(this);
        }

        public override void OnFocus()
        {
            EventSystem.current.SetSelectedGameObject(presetDropdown.dropdown.GetComponentInChildren<Selectable>().gameObject);
        }

        // Update visuals when settings changed

        void UpdatePresetDropdown(ISetting setting)
        {
            var val = (SettingPreset)setting;
            presetDropdown.dropdown.SetSelectedIndexForDataWithoutNotify((int)val.settingQualityLevel);
            Debug.Log(nameof(val) + " = " + val);
        }

        void UpdateVsyncToggle(ISetting setting)
        {
            try
            {
                var val = (VsyncSetting)setting;
                vsyncToggle.toggle.SetIsOnWithoutNotify(val.isOn);
            }
            catch (Exception ex)
            {
                Debug.LogError(nameof(setting) + " = " + setting);
            }
        }

        void UpdateAntiAliasingDropdown(ISetting setting)
        {
            antiAliasDropdown.dropdown.SetSelectedIndexForDataWithoutNotify(setting);
        }

        void UpdateShadowQualityDropdown(ISetting setting)
        {
            shadowQualityDropdown.dropdown.SetSelectedIndexForDataWithoutNotify(setting);
        }

        void UpdateTextureQualityDropdown(ISetting setting)
        {
            textureQualityDropdown.dropdown.SetSelectedIndexForDataWithoutNotify(setting);
        }

        void UpdateResolutionDropdown(ISetting setting)
        {
            resolutionDropdown.dropdown.SetSelectedIndexForDataWithoutNotify(setting);
        }

        void UpdateDisplayTypeDropdown(ISetting setting)
        {
            displayTypeDropdown.dropdown.SetSelectedIndexForDataWithoutNotify(setting);
        }

        // Direct interaction with GraphicSettingContainer - Change settings on value change

        void OnPresetValueChanged(int index)
        {
            var presetIndex = (int)presetDropdown.dropdown.GetData(index).value;
            graphicSettingContainer.ChangePreset(graphicSettingContainer.GetPresetAt(presetIndex));
        }

        void OnResolutionValueChanged(int index)
        {
            var setting = (ResolutionSetting)resolutionDropdown.dropdown.GetData(index).value;
            graphicSettingContainer.ChangeSetting(setting);
        }

        void OnDisplayTypeValueChanged(int index)
        {
            var setting = (DisplayTypeSetting)displayTypeDropdown.dropdown.GetData(index).value;
            graphicSettingContainer.ChangeSetting(setting);
        }

        void OnVsyncValueChanged(bool value)
        {
            graphicSettingContainer.ChangeSetting(new VsyncSetting(value));
        }

        void OnAntiAliasValueChanged(int index)
        {
            var setting = (AntiAliasingSetting)antiAliasDropdown.dropdown.GetData(index).value;
            graphicSettingContainer.ChangeSetting(setting);
        }

        void OnShadowQualityValueChanged(int index)
        {
            var setting = (ShadowQualitySetting)shadowQualityDropdown.dropdown.GetData(index).value;
            graphicSettingContainer.ChangeSetting(setting);
        }

        void OnTextureQualityValueChanged(int index)
        {
            var setting = (TextureQualitySetting)textureQualityDropdown.dropdown.GetData(index).value;
            graphicSettingContainer.ChangeSetting(setting);
        }

        void OnSettingsLoaded(XIVSettings settings)
        {
            graphicSettingContainer?.RemoveListener(this);
            graphicSettingContainer = settings.GetContainer<GraphicSettingContainer>();
            graphicSettingContainer.AddListener(this);
            InitializeUIItems2();
        }

        static TMP_Dropdown.OptionData CreateTmpOptionData(string val) => new TMP_Dropdown.OptionData(val);

        void InitializeUIItems2()
        {
            string[] qualityOptionStrings = new string[]
            {
                "Very Low", "Low", "Medium", "High", "Very High",
            };

            var antiAliasingOptions = ListPool<AntiAliasingSetting>.Get();
            var shadowOptions = ListPool<ShadowQualitySetting>.Get();
            var textureOptions = ListPool<TextureQualitySetting>.Get();
            InitializePresetDropdown(qualityOptionStrings, antiAliasingOptions, shadowOptions, textureOptions);
            InitializeAntiAliasingDropdown(antiAliasingOptions);
            InitializeShadowDropdown(qualityOptionStrings, shadowOptions);
            InitializeTextureDropdown(qualityOptionStrings, textureOptions);
            InitializeResolutionOptions();
            InitializeDisplayType();

            ListPool<AntiAliasingSetting>.Release(antiAliasingOptions);
            ListPool<ShadowQualitySetting>.Release(shadowOptions);
            ListPool<TextureQualitySetting>.Release(textureOptions);

            var preset = graphicSettingContainer.GetSetting<SettingPreset>();
            UpdatePresetDropdown(preset);
            UpdateVsyncToggle(preset.GetSetting<VsyncSetting>());
            UpdateAntiAliasingDropdown(preset.GetSetting<AntiAliasingSetting>());
            UpdateShadowQualityDropdown(preset.GetSetting<ShadowQualitySetting>());
            UpdateTextureQualityDropdown(preset.GetSetting<TextureQualitySetting>());
            UpdateResolutionDropdown(preset.GetSetting<ResolutionSetting>());
            UpdateDisplayTypeDropdown(preset.GetSetting<DisplayTypeSetting>());
        }

        void InitializePresetDropdown(string[] qualityOptionStrings, List<AntiAliasingSetting> antiAliasingOptions, List<ShadowQualitySetting> shadowOptions,
            List<TextureQualitySetting> textureOptions)
        {
            presetDropdown.dropdown.ClearOptions();
            var presetOptionDatas = new List<TMP_Dropdown.OptionData>();
            var presetDropdownOptions = new List<DropdownOptionData<object>>();
            int presetCount = graphicSettingContainer.presetCount;
            for (int i = 0; i < presetCount; i++)
            {
                var preset = graphicSettingContainer.GetPresetAt(i);
                SettingQualityLevel qLevel = preset.settingQualityLevel;
                presetOptionDatas.Add(CreateTmpOptionData(qualityOptionStrings[i]));
                presetDropdownOptions.Add(new DropdownOptionData<object>((int)qLevel));

                var antiAliasing = preset.GetSetting<AntiAliasingSetting>();
                var shadow = preset.GetSetting<ShadowQualitySetting>();
                var texture = preset.GetSetting<TextureQualitySetting>();

                if (antiAliasingOptions.Contains(antiAliasing) == false) antiAliasingOptions.Add(antiAliasing);
                if (shadowOptions.Contains(shadow) == false) shadowOptions.Add(shadow);
                if (textureOptions.Contains(texture) == false) textureOptions.Add(texture);
            }
            presetOptionDatas.Add(CreateTmpOptionData("Custom"));
            presetDropdownOptions.Add(new DropdownOptionData<object>((int)SettingQualityLevel.Custom));
            presetDropdown.dropdown.AddOptions(presetOptionDatas, presetDropdownOptions);
        }

        void InitializeAntiAliasingDropdown(List<AntiAliasingSetting> antiAliasingOptions)
        {
            antiAliasDropdown.dropdown.ClearOptions();
            antiAliasingOptions.Sort((a, b) => a.antiAliasing > b.antiAliasing ? 1 : -1);
            var antiAliasingTmpOptionDatas = new List<TMP_Dropdown.OptionData>();
            var antiAliasingDropdownOptions = new List<DropdownOptionData<object>>();
            int antiAliasingOptionCount = antiAliasingOptions.Count;
            antiAliasingTmpOptionDatas.Add(CreateTmpOptionData("Off"));
            antiAliasingDropdownOptions.Add(new DropdownOptionData<object>(antiAliasingOptions[0]));
            for (int i = 1; i < antiAliasingOptionCount; i++)
            {
                var antiAliasingSetting = antiAliasingOptions[i];
                antiAliasingTmpOptionDatas.Add(CreateTmpOptionData(antiAliasingSetting.antiAliasing + "x MSAA"));
                antiAliasingDropdownOptions.Add(new DropdownOptionData<object>(antiAliasingSetting));
            }
            antiAliasDropdown.dropdown.AddOptions(antiAliasingTmpOptionDatas, antiAliasingDropdownOptions);
        }

        void InitializeShadowDropdown(string[] qualityOptionStrings, List<ShadowQualitySetting> shadowOptions)
        {
            shadowQualityDropdown.dropdown.ClearOptions();
            //shadowOptions.Sort((a, b) => (int)a.shadowResolution > (int)b.shadowResolution ? 1 : -1);
            var shadowTmpOptionDatas = new List<TMP_Dropdown.OptionData>();
            var shadowDropdownOptions = new List<DropdownOptionData<object>>();
            int shadowOptionCount = shadowOptions.Count;
            for (int i = 0; i < shadowOptionCount; i++)
            {
                var shadowQualitySetting = shadowOptions[i];
                shadowTmpOptionDatas.Add(CreateTmpOptionData(qualityOptionStrings[i]));
                shadowDropdownOptions.Add(new DropdownOptionData<object>(shadowQualitySetting));
            }
            shadowQualityDropdown.dropdown.AddOptions(shadowTmpOptionDatas, shadowDropdownOptions);
        }

        void InitializeTextureDropdown(string[] qualityOptionStrings, List<TextureQualitySetting> textureOptions)
        {
            textureQualityDropdown.dropdown.ClearOptions();
            textureOptions.Sort((a, b) => a.masterTextureLimit < b.masterTextureLimit ? 1 : -1);
            var textureTmpOptionDatas = new List<TMP_Dropdown.OptionData>();
            var textureDropdownOptions = new List<DropdownOptionData<object>>();
            int textureOptionCount = textureOptions.Count;
            for (int i = 0; i < textureOptionCount; i++)
            {
                var textureQualitySetting = textureOptions[i];
                textureTmpOptionDatas.Add(CreateTmpOptionData(qualityOptionStrings[i]));
                textureDropdownOptions.Add(new DropdownOptionData<object>(textureQualitySetting));
            }
            textureQualityDropdown.dropdown.AddOptions(textureTmpOptionDatas, textureDropdownOptions);
        }

        void InitializeResolutionOptions()
        {
            resolutionDropdown.dropdown.ClearOptions();
            var resolutionOptions = new List<TMP_Dropdown.OptionData>();
            var bindedDatas = new List<DropdownOptionData<object>>();
            var resolutions = Screen.resolutions;
            int length = resolutions.Length;
            for (int i = 0; i < length; i++)
            {
                var res = resolutions[i];
                var resText = res.width + "x" + res.height;
                resolutionOptions.Add(new TMP_Dropdown.OptionData(resText));
                bindedDatas.Add(new DropdownOptionData<object>(new ResolutionSetting(res.width, res.height)));
            }
            resolutionDropdown.dropdown.AddOptions(resolutionOptions, bindedDatas);
        }

        void InitializeDisplayType()
        {
            displayTypeDropdown.dropdown.ClearOptions();
            var optionDatas = new List<TMP_Dropdown.OptionData>
            {
                new TMP_Dropdown.OptionData("FullScreen"),
                new TMP_Dropdown.OptionData("Windowed"),
            };
            var datas = new List<DropdownOptionData<object>> 
            {
                new DropdownOptionData<object>(new DisplayTypeSetting(true)),
                new DropdownOptionData<object>(new DisplayTypeSetting(false)), 
            };
            displayTypeDropdown.dropdown.AddOptions(optionDatas, datas);
        }

        void ISettingListener.OnSettingChanged(SettingChange settingChange)
        {
            onSettingChangeLookup[settingChange.settingType].Invoke(settingChange.to);
        }

        void ISettingListener.OnBeforeApply(ISettingContainer _) { }

        void ISettingListener.OnAfterApply(ISettingContainer _) { }
    }
}