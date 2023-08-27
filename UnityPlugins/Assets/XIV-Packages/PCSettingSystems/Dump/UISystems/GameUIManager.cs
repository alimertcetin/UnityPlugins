using TheGame.SceneManagement;
using TheGame.ScriptableObjects.Channels;
using TheGame.UISystems.Core;
using TheGame.UISystems.SceneLoading;
using UnityEngine;

namespace TheGame.UISystems
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] SceneLoadChannelSO displayLoadingScreenChannel;
        [SerializeField] VoidChannelSO stopDisplayingLoadingScreenChannel;
        [SerializeField] BoolChannelSO showPauseUIChannel;

        void OnEnable()
        {
            displayLoadingScreenChannel.Register(OnDisplayLoadingScreen);
            stopDisplayingLoadingScreenChannel.Register(OnStopDisplayingLoadingScreen);
            showPauseUIChannel.Register(OnShowPauseUI);
        }

        void OnDisable()
        {
            displayLoadingScreenChannel.Unregister(OnDisplayLoadingScreen);
            stopDisplayingLoadingScreenChannel.Unregister(OnStopDisplayingLoadingScreen);
            showPauseUIChannel.Unregister(OnShowPauseUI);
        }

        void OnShowPauseUI(bool val)
        {
            //// Pause UI raises an event to inform other systems that game is paused or not
            //if (val)
            //{
            //    UISystem.Hide<HudUI>();
            //    UISystem.Show<PauseUI>();
            //}
            //else
            //{
            //    UISystem.Show<HudUI>();
            //    UISystem.Hide<PauseUI>();
            //}
        }

        void OnDisplayLoadingScreen(SceneLoadOptions sceneLoadOptions)
        {
            var sceneLoadingUI = UISystem.GetUI<SceneLoadingUI>();
            sceneLoadingUI.SetSceneLoadingOptions(sceneLoadOptions);
            sceneLoadingUI.Show();
        }

        void OnStopDisplayingLoadingScreen()
        {
            UISystem.Hide<SceneLoadingUI>();
        }
    }
}