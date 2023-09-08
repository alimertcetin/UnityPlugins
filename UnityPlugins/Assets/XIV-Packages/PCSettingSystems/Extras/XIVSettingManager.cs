using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XIV.Core;
using XIV.XIVEditor.Utils;
using XIV_Packages.PCSettingsSystem;

namespace Assets.XIV
{
    public struct UndoCommand : ICommand
    {
        ISettingContainer settingContainer;

        public UndoCommand(ISettingContainer settingContainer)
        {
            this.settingContainer = settingContainer;
        }

        void ICommand.Execute()
        {
            settingContainer.Undo();
        }

        void ICommand.Unexecute()
        {
            settingContainer.Redo();
        }
    }
    public struct RedoCommand : ICommand
    {
        ISettingContainer settingContainer;

        public RedoCommand(ISettingContainer settingContainer)
        {
            this.settingContainer = settingContainer;
        }

        void ICommand.Execute()
        {
            settingContainer.Redo();
        }

        void ICommand.Unexecute()
        {
            settingContainer.Undo();
        }
    }

    public class XIVSettingManager : MonoBehaviour, ISettingListener
    {
        public XIVSettings settings;
        public SettingsChannelSO settingLoadedChannel;
        public TMP_Text txt_Timer;
        public List<string> undoHistory;
        [SerializeField, DisplayWithoutEdit] SettingManager[] settingManagers;

        bool hasCriticalChange;
        ISettingContainer settingContiner;
        float timer;
        UndoRedoStack<ICommand> commands = new UndoRedoStack<ICommand>();
        UndoRedoStack<ICommand> graphicContainerComamnds;

        void Awake()
        {
            settings = new XIVSettings();
            int length = settingManagers.Length;
            for (int i = 0;  i < length;  i++)
            {
                var settingManager = settingManagers[i];
                settingManager.InitializeContainer();
                settings.AddContainer(settingManager.GetContainer());
            }

            graphicContainerComamnds = ReflectionUtils.GetFieldValue<UndoRedoStack<ICommand>>("commands", settings.GetContainer<GraphicSettingContainer>());
        }

        void Start()
        {
            settingLoadedChannel.RaiseEvent(settings);
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

        void Update()
        {
            var graphicContainer = settings.GetContainer<GraphicSettingContainer>();

            if (Input.GetKeyDown(KeyCode.T))
            {
                if (graphicContainer.HasUnappliedChange() == false)
                {
                    Debug.Log("There is no change to apply");
                    return;
                }
                graphicContainer.ApplyChanges();
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                commands.Do(new UndoCommand(graphicContainer));
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                commands.Do(new RedoCommand(graphicContainer));
            }

            undoHistory.Clear();

            foreach (var item in graphicContainerComamnds.GetUndoOperations())
            {
                undoHistory.Add(item.ToString().Split('.')[^1]);
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
            if (commands.undoCount > 0) commands.Undo();
            else settingContiner.Undo();
            ResetState();
        }

        void ResetState()
        {
            settingContiner = default;
            hasCriticalChange = false;
            timer = 0f;
            txt_Timer.text = timer.ToString("F1");
        }

        void ISettingListener.OnSettingChanged(SettingChange _) { }

        void ISettingListener.OnBeforeApply(ISettingContainer container)
        {
            if (settingContiner != default || container is not GraphicSettingContainer graphicSettingContainer) return;

            foreach (SettingChange settingchange in graphicSettingContainer.GetUnappliedSettings())
            {
                if (settingchange.to.IsCritical == false) continue;

                settingContiner = container;
                hasCriticalChange = true;
                Debug.Log(nameof(settingchange.to) + " = " + settingchange.to);
                break;
            }
        }

        void ISettingListener.OnAfterApply(ISettingContainer _) { }

#if UNITY_EDITOR
        void OnValidate()
        {
            settingManagers = GetComponentsInChildren<SettingManager>();
        }
#endif

    }
}