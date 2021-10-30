/*
 * Hypercasual Game Template
 * 
 * This is a hypercasual game template developed on the Pixel Framework with the support of TinyPlay.
 * This template and its components will help you quickly create hypercasual games,
 * as it includes everything you need to get started quickly.
 *
 * You can read more about Incubator at the official website:
 * https://pixinc.club/
 *
 * @developer       Ilya Rastorguev
 * @autor           TinyPlay, Inc.
 * @version         1.0
 * @url             https://pixinc.club/
 * @support         https://github.com/TinyPlay/PixelFramework/issues
 * @discord         https://discord.gg/wE67T7Vm
 */

using PixelFramework.Managers;

namespace HyperSample.UI.Views
{
    using UnityEngine;
    using PixelFramework.UI.View;
    using UnityEngine.Events;
    using HyperSample.Models;
    
    /// <summary>
    /// Main Menu Controller
    /// </summary>
    public class MainMenuPm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
            public UnityEvent<bool> PlayButtonClicked = new UnityEvent<bool>();
            public UnityEvent SettingsButtonClicked = new UnityEvent();
            public UnityEvent StoreButtonClicked = new UnityEvent();
            
            public Transform ViewParent;
            public GameObject ViewPrefab;
        }

        /// <summary>
        /// On Context Initialized
        /// </summary>
        public override void OnContextInitialized()
        {
            // Get Context
            Context ctx = (Context) GetContext();
            GameStateModel gameData = (GameStateModel) GameManager.Instance().GetCurrentState();
            
            // Initialize View
            GameObject viewPrefab = GameObject.Instantiate(ctx.ViewPrefab, ctx.ViewParent);
            MainMenuView view = viewPrefab.GetComponent<MainMenuView>();
            
            // Initialize and Show View
            view.SetContext(new MainMenuView.Context()
            {
                OnPlayButtonClicked = ctx.PlayButtonClicked,
                OnSettingsButtonClicked = ctx.SettingsButtonClicked,
                OnStoreButtonClicked = ctx.StoreButtonClicked,
                
                CoinsValue = gameData.HardCurrency,
                StarsValue = gameData.SoftCurrency
            }).ShowView();
        }
    }
}