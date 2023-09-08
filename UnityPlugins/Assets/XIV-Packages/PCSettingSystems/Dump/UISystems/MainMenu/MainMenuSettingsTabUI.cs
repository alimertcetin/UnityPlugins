﻿using TheGame.UISystems.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.XIV
{
    public class MainMenuSettingsTabUI : TabUI
    {
        [SerializeField] Button btn_back;

        protected override void OnEnable()
        {
            base.OnEnable();
            btn_back.onClick.AddListener(GoBackToMainPage);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            btn_back.onClick.RemoveAllListeners();
        }

        public override void Hide()
        {
            base.Hide();
            EventSystem.current.SetSelectedGameObject(null);
        }

        protected override void OnUIActivated()
        {
            base.OnUIActivated();
            EventSystem.current.SetSelectedGameObject(pages[0].tabButton.gameObject);
        }

        void GoBackToMainPage()
        {
            UISystem.Hide<MainMenuSettingsTabUI>();
            UISystem.Show<MainMenuUI>();
        }
    }
}