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
    /// Pause Menu Controller
    /// </summary>
    public class PausePm : BasePm
    {
        [System.Serializable]
        public class Context : IPmContext
        {
            public UnityEvent<bool> GamePaused = new UnityEvent<bool>();
            public UnityEvent OnMenuExit = new UnityEvent();

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
            PauseView view = viewPrefab.GetComponent<PauseView>();

            view.SetContext(new PauseView.Context()
            {
                OnGamePause = ctx.GamePaused,
                OnMainMenuExit = () =>
                {
                    ctx.OnMenuExit.Invoke();
                }
            });
        }
    }
}