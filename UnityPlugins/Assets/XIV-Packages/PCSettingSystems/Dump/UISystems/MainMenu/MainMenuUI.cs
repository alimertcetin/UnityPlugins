using Assets.XIV;
using UnityEngine;
using UnityEngine.EventSystems;
using XIV.Core.TweenSystem;
using XIV.Core.Utils;
using XIV_Packages.SaveSystems;

namespace TheGame.UISystems.MainMenu
{
    public class MainMenuUI : GameUI
    {
        [Header("Main Page UI Elements")]
        [SerializeField] CustomButton btn_Start;
        [SerializeField] CustomButton btn_Continue;
        [SerializeField] CustomButton btn_Settings;
        [SerializeField] CustomButton btn_Exit;

        GameObject lastClickedButton;

        void Start()
        {
            Show();
        }

        void OnEnable()
        {
            btn_Settings.onClick.AddListener(ShowSettingsPage);
            btn_Exit.onClick.AddListener(ExitGame);

            btn_Start.RegisterOnSelect(OnButtonPressed);
            btn_Continue.RegisterOnSelect(OnButtonPressed);
            btn_Settings.RegisterOnSelect(OnButtonPressed);
            btn_Exit.RegisterOnSelect(OnButtonPressed);
        }

        void OnDisable()
        {
            btn_Settings.onClick.RemoveListener(ShowSettingsPage);
            btn_Exit.onClick.RemoveListener(ExitGame);

            btn_Start.UnregisterOnSelect();
            btn_Continue.UnregisterOnSelect();
            btn_Settings.UnregisterOnSelect();
            btn_Exit.UnregisterOnSelect();
        }
        
        public override void Show()
        {
            var upPos = Vector2.up * (uiGameObjectRectTransform.rect.height + uiGameObjectRectTransform.offsetMin.y + 10f);
            uiGameObjectRectTransform.CancelTween();
            uiGameObjectRectTransform.anchoredPosition = upPos;
            
            uiGameObject.transform.XIVTween()
                .RectTransformMove(upPos, Vector2.zero, 0.5f, EasingFunction.EaseOutExpo)
                .UseUnscaledDeltaTime()
                .OnComplete(() =>
                {
                    isActive = true;
                    OnUIActivated();
                })
                .Start();
            uiGameObject.SetActive(true);
        }
        
        public override void Hide()
        {
            EventSystem.current.SetSelectedGameObject(null);
            
            uiGameObjectRectTransform.CancelTween();

            var upPos = Vector2.up * (uiGameObjectRectTransform.rect.height + uiGameObjectRectTransform.offsetMin.y + 10f);
            
            uiGameObject.transform.XIVTween()
                .RectTransformMove(Vector2.zero, upPos, 0.5f, EasingFunction.EaseOutExpo)
                .UseUnscaledDeltaTime()
                .OnComplete(() =>
                {
                    uiGameObject.SetActive(false);
                    isActive = false;
                    OnUIDeactivated();
                })
                .Start();
        }

        protected override void OnUIActivated()
        {
            EventSystem.current.SetSelectedGameObject(lastClickedButton ? lastClickedButton : btn_Start.gameObject);
        }

        void ShowSettingsPage()
        {
            lastClickedButton = btn_Settings.gameObject;
            PlayButtonPressSound();
            UISystem.Hide<MainMenuUI>();
            UISystem.Show<MainMenuSettingsTabUI>();
        }

        void ExitGame()
        {
            PlayButtonPressSound();
            Application.Quit();
        }

        void OnButtonPressed()
        {
            //mainMenuButtonSelectionAudioPlayer.Play();
        }

        void PlayButtonPressSound()
        {
            //mainMenuButtonPressAudioPlayer.Play();
        }
    }
}