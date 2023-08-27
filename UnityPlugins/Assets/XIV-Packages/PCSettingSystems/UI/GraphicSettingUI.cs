using UnityEngine;
using UnityEngine.UI;

namespace XIV_Packages.PCSettingsSystem
{
    public class GraphicSettingUI : MonoBehaviour, ISettingListener
    {
        public Button buttonGO;

        Button[] buttons;
        GraphicSettingContainer graphicSettingContainer;

        void Start()
        {
            var settingManager = FindObjectOfType<XIVSettingManager>();

            graphicSettingContainer = settingManager.settings.GetContainer<GraphicSettingContainer>();
            int count = graphicSettingContainer.presetCount;

            buttons = new Button[count];

            for (int i = 0; i < count; i++)
            {
                var preset = graphicSettingContainer.GetPresetAt(i);
                var btn = Object.Instantiate(buttonGO);
                btn.GetComponentInChildren<Text>().text = preset.settingQualityLevel.ToString();
                btn.transform.SetParent(buttonGO.transform.parent);
                buttons[i] = btn;
            }
            Destroy(buttonGO.gameObject);
        }

        void OnEnable()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                var btn = buttons[i];
                var index = i;
                btn.onClick.AddListener(() => 
                {
                    //ChangePreset(index);
                });
            }
            graphicSettingContainer.AddListener(this);
        }

        void OnDisable()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].onClick.RemoveAllListeners();
            }
            graphicSettingContainer.RemoveListener(this);
        }

        void ISettingListener.OnSettingChanged(SettingChange settingChange)
        {

        }

        void ISettingListener.OnBeforeApply(ISettingContainer _)
        {

        }

        void ISettingListener.OnAfterApply(ISettingContainer _)
        {

        }
    }
}