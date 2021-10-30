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
namespace HyperSample.UI.Views
{
    using UnityEngine;
    using PixelFramework.UI.View;
    using UnityEngine.Events;
    using PixelFramework.Managers;
    
    /// <summary>
    /// Main UI Controller
    /// </summary>
    public class MainUIPm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
            public UnityEvent<bool> GamePaused = new UnityEvent<bool>();
            public UnityEvent<int> OnCoinColleted = new UnityEvent<int>();

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
            
            // Initialize View
            GameObject viewPrefab = GameObject.Instantiate(ctx.ViewPrefab, ctx.ViewParent);
            MainUIView view = viewPrefab.GetComponent<MainUIView>();

            view.SetContext(new MainUIView.Context()
            {
                OnGamePause = ctx.GamePaused,
                OnCoinsProgress = ctx.OnCoinColleted
            }).ShowView();
        }
    }
}